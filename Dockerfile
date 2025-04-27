# Use official .NET 8 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Argus.InventoryService.csproj", "./"]
RUN dotnet restore "./Argus.InventoryService.csproj"
COPY . .
RUN dotnet publish "./Argus.InventoryService.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Argus.InventoryService.dll"]
