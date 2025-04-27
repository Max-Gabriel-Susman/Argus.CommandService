
build:
	docker build -t argus-inventory-service .

run:
	docker run -d -p 8080:8080 --name argus-container argus-inventory-service

scaffold:
	dotnet aspnet-codegenerator controller -name InventoryItemsController -async -api -m InventoryItem -dc InventoryContext -outDir Controllers