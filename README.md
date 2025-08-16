# Vertical Slices Starter — EF Core (Write + Outbox) + RabbitMQ + Mongo (Read)

Features live in **YourApp.Api/Features/**. Shared stuff in **Core** and **Infrastructure**.

## Build & Run
```bash
dotnet new sln -n YourApp
dotnet sln add src/Core/Core.csproj
dotnet sln add src/Infrastructure/Infrastructure.csproj
dotnet sln add src/YourApp.Api/YourApp.Api.csproj

dotnet restore
dotnet run --project src/YourApp.Api/YourApp.Api.csproj
```
Endpoints:
- `POST /products` — writes to SQL and Outbox within the same transaction
- `GET /products?category=Peripherals` — reads from Mongo read model

Background worker (`OutboxHostedService`) publishes Outbox to RabbitMQ exchange `domain-events`.
