# PlatformFoundation

Phase 1: Platform Foundation (ASP.NET Core)

See docs/architecture.md for rules and layering.

## Run locally

### Prerequisites
- .NET SDK 8+

### Start the API
```bash
dotnet run --project PlatformFoundation.WebApi
```

### App info
- `GET /api/info` returns app metadata (environment/version) for debugging deployments.
