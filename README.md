# Argus Inventory Service

Argus Inventory Service is the backend for the Argus inventory management system. This service exposes a set of CRUD operations for the inventories persisted in the MS SQL Server instance.

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

Create your first inventory item: 
```
curl -X 'POST' \
  'http://localhost:8080/api/InventoryItems' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "hello argus",
  "isComplete": true
}'
```

Retrieve the inventory: 
```
curl -X 'GET' \
  'http://localhost:8080/api/InventoryItems' \
  -H 'accept: text/plain'
```
