# WebAPI

Minimal Clean Architecture sample built with .NET 10, FastEndpoints, EF Core, and Aspire-friendly service defaults.

## Layout

```text
.
+-- docker/
|   +-- web/
|       +-- Dockerfile
+-- docker-compose.yml
+-- src/
|   +-- WebAPI.Web/
|   +-- WebAPI.AspireHost/
|   +-- WebAPI.ServiceDefaults/
+-- WebAPI.slnx
```

The Docker-related files live one level above the application project so the repo stays organized as the stack grows.

## Run Locally

```powershell
dotnet build
dotnet run --project src/WebAPI.Web
```

If you want the Aspire host instead:

```powershell
dotnet run --project src/WebAPI.AspireHost
```

## Run With Docker

1. Copy `.env.example` to `.env`.
2. Set a strong `MSSQL_SA_PASSWORD`.
3. Start the stack:

```powershell
docker compose up --build
```

Services started by Compose:

- SQL Server 2025 latest from `mcr.microsoft.com/mssql/server:2025-latest`
- The web API container
- Papercut for local email testing

The API is exposed on `http://localhost:8080`.

## Notes

- The Compose stack sets `ConnectionStrings__AppDb` for the web container.
- The app still supports direct local execution with the `ConnectionStrings:AppDb` value in `src/WebAPI.Web/appsettings.json`.
- SQL Server uses a persistent named volume so data survives container restarts.

## Development

The project is organized by vertical slice features under `src/WebAPI.Web/`.

Main areas:

- `Domain/` for entities and aggregates
- `Infrastructure/` for persistence and external services
- `CartFeatures/`, `ProductFeatures/`, and `AiFeatures/` for feature slices
- `Program.cs` for startup composition

## Database

When running with Docker Compose, SQL Server is available at:

```text
Server=sqlserver,1433;Database=MinimalCleanArchitecture;User Id=sa;Password=<your password>;TrustServerCertificate=True;Encrypt=False
```

The database is created and migrated automatically on startup.
