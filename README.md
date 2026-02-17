# PlatformFoundation

A clean architecture ASP.NET Core 8 Web API for platform foundations, with consistent error responses, correlation IDs, structured request logging, EF Core + PostgreSQL persistence, and integration tests.

## 5-minute quickstart

```bash
# 1) Start PostgreSQL
docker compose up -d postgres

# 2) Restore/build
dotnet restore
dotnet build

# 3) Apply migrations
dotnet ef database update \
  --project PlatformFoundation.Infrastructure \
  --startup-project PlatformFoundation.WebApi

# 4) Run API
dotnet run --project PlatformFoundation.WebApi
```

Base URLs (Development profile):
- `https://localhost:7048`
- `http://localhost:5064`

Smoke checks:
```bash
curl -k https://localhost:7048/api/info
curl -k https://localhost:7048/health/live
curl -k https://localhost:7048/health/ready
```

## Overview

Solution projects:
- `PlatformFoundation.Domain`: domain entities and domain exceptions.
- `PlatformFoundation.Application`: use-case handlers and contracts.
- `PlatformFoundation.Infrastructure`: EF Core, PostgreSQL, repositories, migrations.
- `PlatformFoundation.WebApi`: controllers, middleware, HTTP contracts, pipeline.
- `PlatformFoundation.IntegrationTests`: end-to-end API tests with Testcontainers PostgreSQL.

## Architecture & Layering

Dependency direction:
- `Domain <- Application <- Infrastructure <- WebApi`

Key implementation notes:
- WebApi orchestrates use-cases only; no direct EF usage in controllers.
- Infrastructure implements repository and transaction contracts.
- `IUnitOfWork` is implemented by `EfUnitOfWork` and commits with `SaveChangesAsync`.
- `CreateProductHandler` writes both `Product` and `ProductAuditLog` (Day 19) in one unit of work.

Boundary checks are enforced by script:
```bash
./scripts/verify-boundaries.sh
```

## Request Pipeline Order

Configured in `PlatformFoundation.WebApi/Program.cs`:
1. `CorrelationIdMiddleware`
2. `UseRateLimiter`
3. `UseSerilogRequestLogging`
4. `ExceptionHandlingMiddleware`
5. `UseHttpsRedirection`
6. `UseAuthorization`
7. `MapControllers`
8. `MapHealthChecks` (`/health/live`, `/health/ready`)

Additional behavior:
- `X-Correlation-ID` request header is accepted/created and echoed in response.
- Serilog request logging includes correlation ID, endpoint, trace identifier, client IP, and user agent.
- `ExceptionHandlingMiddleware` converts unhandled domain/system exceptions to API error payloads.

## Error Contract

All API errors use `ErrorResponse` shape via `ErrorFactory`:

```json
{
  "traceId": "string",
  "status": 400,
  "title": "Validation failed",
  "detail": "One or more validation errors occurred.",
  "errors": {
    "Name": ["The Name field is required."]
  }
}
```

Typical mappings:
- `400` validation (model or domain validation)
- `404` not found
- `409` conflict (e.g., duplicate product name)
- `429` too many requests
- `500` unexpected server errors

## Endpoints

Base path examples use `https://localhost:7048`.

- `GET /api/info`
  - App metadata (name, environment, version, UTC time).

- `POST /api/products` (rate-limited with `write-strict`)
  - Body:
    ```json
    { "name": "Coffee", "price": 2.5 }
    ```
  - `201 Created` with `Location: /api/products/{id}`.

- `GET /api/products/{id}`
  - `200 OK` with product.
  - `404 Not found` if missing.

- `GET /api/products?limit=20&offset=0`
  - Paged list response.

- `GET /api/ping`
  - Basic ping response from application handler.

- `GET /health/live`
- `GET /health/ready`

## Local Run

```bash
docker compose up -d postgres
dotnet run --project PlatformFoundation.WebApi
```

Optional correlation ID header example:
```bash
curl -k https://localhost:7048/api/products \
  -H 'Content-Type: application/json' \
  -H 'X-Correlation-ID: local-dev-001' \
  -d '{"name":"Notebook","price":9.99}'
```

## Database & Migrations

Default PostgreSQL connection (from `appsettings*.json`):
- Host `localhost`, Port `5432`, DB `platformfoundation`, User `pf_user`, Password `pf_pass`

Start DB:
```bash
docker compose up -d postgres
```

Apply existing migrations:
```bash
dotnet ef database update \
  --project PlatformFoundation.Infrastructure \
  --startup-project PlatformFoundation.WebApi
```

Create a new migration:
```bash
dotnet ef migrations add <MigrationName> \
  --project PlatformFoundation.Infrastructure \
  --startup-project PlatformFoundation.WebApi \
  --output-dir Persistence/Migrations
```

## Health Checks

- `/health/live`: liveness probe (`self` check).
- `/health/ready`: readiness probe (`self` check).

Both endpoints return JSON and are suitable for container/orchestrator probes.

## Rate Limiting

Configured with ASP.NET Core rate limiting:
- Global limiter: `60` requests/minute per client IP.
- Named policy `write-strict`: `10` requests/minute.
- `POST /api/products` uses `write-strict` via `[EnableRateLimiting("write-strict")]`.
- Rejections return `429` using `ErrorFactory.TooManyRequests(...)`.

## Integration Tests

`PlatformFoundation.IntegrationTests` uses:
- `WebApplicationFactory<Program>`
- `Testcontainers.PostgreSql`
- Runtime migration apply in test setup (`db.Database.MigrateAsync()`)

Run all tests:
```bash
dotnet test
```

Run integration tests only:
```bash
dotnet test PlatformFoundation.IntegrationTests/PlatformFoundation.IntegrationTests.csproj
```

## CI

GitHub Actions workflow: `.github/workflows/ci.yml`

Runs on push to `main` and pull requests:
1. Checkout
2. Setup .NET 8
3. Install `ripgrep`
4. Run boundary checks (`./scripts/verify-boundaries.sh`)
5. `dotnet build`
6. `dotnet test`

## Troubleshooting

- `Failed to connect to PostgreSQL`:
  - Ensure DB is running: `docker compose ps`
  - Restart DB: `docker compose up -d postgres`

- `dotnet ef` not found:
  - Install tool: `dotnet tool install --global dotnet-ef`

- HTTPS certificate warning locally:
  - Use `-k` in curl for local HTTPS or trust local certs.

- Getting `429 Too Many Requests` during local testing:
  - Wait for the limiter window (1 minute) or reduce request burst.

- Correlation ID missing in client logs:
  - Send `X-Correlation-ID` request header and verify response includes it.
