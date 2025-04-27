
build:
	docker build -t argus-inventory-service .

run:
	docker run -d -p 8080:8080 --name argus-container argus-inventory-service
