# Argus Command Service

Argus Command Service is the backend for the Argus fleet management system. It's dockerized and can be spun up locally using the included `docker-compose.yaml` file and following the instructions under the `Local Setup` section of this README. Additionally the instructions for deploying this to EKS are included in the `EKS Deployment` section of this README and it would be a fairly low level of effort to deploy to another cloud platform like Linode or Azure.

Argus Command Service is not currently productionalized, there are a handful of requirements that are unmet including: 

* The data persisted in the SQL Server instance is currently ephemeral you'd want to use a solution like Amazon RDS to resolve this.

* The SQL Server instance's passwords are sensitive values and need to be stored in a Secrets Manager.

* The service would need a proper domain name and SSL certificates for a production setup.

* The service is currenlty always in development mode. Where this exposes swagger publicly we would either need to make development mode conditional or we'd need to make this service private so as to not expose the swagger endpoint publicly.

## Local Setup

Spin up the service: 
```
docker compose up -d
```

Install EF Core CLI tools if not already installed:
```
dotnet tool install --global dotnet-ef
```

Apply Database Migrations: 
```
dotnet ef migrations add InitialCreate
dotnet ef database update

```

These two methods and several more can be interacted with more easily by navigating to http://localhost:8080/swagger/index.html while the service is running.

## EKS Deployment

Local deployment is great for development and playing around with Argus' backend, however to use Argus with an Argus client in real world scenarios you'll want to make it publicly accessible. There's a lot of ways to do this but the recommended approach is deploying Argus Command Service to an Amazon Elastic Kubernetes Service(EKS) cluster. The instructions for deploying to EKS are outlined below:

Prerequisites:

* AWS CLI installed: [AWS CLI docs]()

* AWS CLI configured: `aws configure`

* kubectl installed: [kubectl docs](https://kubernetes.io/docs/tasks/tools/)

* eksctl installed: [eksctl docs](https://eksctl.io/installation/)

* Docker Hub or Amazon ECR for container registry(in the guide below we'll use ECR)

1. Push to Amazon Elastic Container Registry:

Create ECR repository:
```
aws ecr create-repository --repository-name argus-command-service

```

Authenticate Docker to ECR:
```
aws ecr get-login-password | docker login --username AWS --password-stdin <aws_account_id>.dkr.ecr.<region>.amazonaws.com
```

Tag and Push your Image: 
```
docker build -t argus-command-service .
docker tag argus-command-service:latest <aws_account_id>.dkr.ecr.<region>.amazonaws.com/argus-command-service:latest
docker push <aws_account_id>.dkr.ecr.<region>.amazonaws.com/argus-command-service:latest
```

2. Create Argus EKS Cluster:

Create the EKS Cluster for Argus: 
```
eksctl create cluster --name argus-cluster --region <region> --nodegroup-name argus-nodes --nodes 2 --nodes-min 1 --nodes-max 3 --managed
```

3. Deploy the Argus Command Service DB:

Apply the `argus-command-service-db.yml` manifest included in this repository: 
```
kubectl apply -f argus-command-service-db.yml
```

4. Deploy the Argus Command Service:

Apply the `argus-command-service.yml` manifest included in this repository: 
```
kubectl apply -f argus-command-service.yml
```

5. Access Your Public API:

Get your public Load Balancer URL:
```
kubectl get svc argus-command-service
```

Test with cURL:
```
curl http://<EXTERNAL_IP>/api/CommandItems
```

Or with Swagger in browser like: 
```
http://<EXTERNAL_IP>/swagger
```

6. Connecting with an Argus Client:

