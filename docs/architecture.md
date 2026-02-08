# Architecture Rules (Phase 1)

## Purpose
This solution is designed to keep platform code consistent and maintainable.
We enforce boundaries so the system doesn’t become “controller + EF + random logic”.

## Projects and Responsibilities

### Domain
**Owns business rules and invariants**
- Entities, Value Objects
- Domain exceptions
- Domain enums/constants
- NO EF Core, NO HTTP, NO logging frameworks

### Application
**Owns use-cases (business workflows)**
- Commands/Queries + handlers
- Interfaces (ports) that infrastructure implements (e.g., repositories)
- Validation policy (TBD: WebApi or Application, but must be consistent)
- NO EF Core, NO HttpContext

### Infrastructure
**Owns technical implementations**
- EF Core DbContext + migrations
- Repository implementations
- External services (email, redis, etc.)
- Can reference EF Core and database code

### WebApi
**Owns HTTP and request pipeline**
- Controllers / Minimal API endpoints
- Middleware pipeline
- Request/response contracts
- NO EF Core usage directly
- NO business rules here

## Dependency Rule (must never be broken)
Domain ← Application ← Infrastructure ← WebApi

- Domain depends on nothing
- Application depends only on Domain
- Infrastructure depends on Application + Domain
- WebApi depends on Application + Infrastructure

## Coding Rules (non-negotiable)
1. Controllers do not call DbContext or EF directly.
2. Controllers do not contain business logic (only orchestration).
3. Domain invariants are enforced in Domain (constructors/factories/guard methods).
4. Application defines interfaces; Infrastructure implements them.
5. Errors are returned in one consistent format (Phase 1 will define it).
6. Logging policy: errors logged once at the boundary (middleware), not everywhere.

## Folder Convention (starting point)
- Application: Features/{FeatureName}/Commands|Queries/{UseCaseName}/
- WebApi: Contracts/Requests + Contracts/Responses
- Domain: Entities, ValueObjects, Exceptions
- Infrastructure: Persistence, Repositories

## API Contracts & Mapping Rules

### WebApi Contracts (HTTP boundary)
- WebApi owns HTTP contracts:
    - Requests: WebApi/Contracts/Requests
    - Responses: WebApi/Contracts/Responses
- WebApi contracts are NOT referenced by Application/Domain/Infrastructure.

### Application Results (use-case outputs)
- Application returns Result models (not IActionResult, not WebApi DTOs).
- Result models are stable and represent the outcome of a use-case.

### Mapping rule
- Controllers map:
    - Request DTO -> Application Command/Query
    - Application Result -> Response DTO
- No mapping logic lives in Application (unless it’s domain/business transformation).

### Naming conventions
- Application:
    - Commands: CreateXCommand / UpdateXCommand
    - Queries: GetXQuery / ListXQuery
    - Handlers: CreateXHandler / GetXHandler
    - Results: XResult / CreateXResult
- WebApi:
    - Requests: CreateXRequest / UpdateXRequest
    - Responses: XResponse / XDetailsResponse

### Log behavior matrix
- 2xx: request log only (INF)
- 4xx validation: request log only (INF)
- 5xx exception: request log (INF) + exception log once (ERR) in exception middleware
