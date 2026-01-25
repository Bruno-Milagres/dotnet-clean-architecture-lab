# .NET Clean Architecture – Reference Template

## Purpose
This repository is an **opinionated Clean Architecture foundation for modern .NET applications**.

It was created as:
- a **personal reference** for architectural decisions
- a **reusable project starter** (template mindset, not a framework)
- a **learning log** that evolves with real-world experience

The focus is **clarity over boilerplate**, **pragmatism over dogma**, and **patterns that scale from small APIs to complex systems**.

---

## Architectural Philosophy

This project follows **Clean Architecture principles**, inspired by:
- Microsoft .NET Architecture Guidelines
- Jason Taylor (Clean Architecture)
- Steve Smith (Ardalis)
- Pragmatic DDD and Vertical Slice Architecture

Core ideas:
- **Domain is king** – pure, isolated, framework-agnostic
- **Application defines use cases**, not technical details
- **Infrastructure implements contracts**, never leaks inward
- **WebApi is only an entry point**, thin and disposable

Dependencies flow **inward only**.

```
WebApi → Application → Domain
        ↑
   Infrastructure
```

---

## Solution Structure

```
MyProject
├── MyProject.Domain
│   ├── Entities
│   ├── ValueObjects
│   └── Exceptions
│
├── MyProject.Application
│   ├── Features
│   ├── Behaviors
│   └── Abstractions
│       ├── Persistence
│       ├── Messaging
│       └── Services
│
├── MyProject.Infrastructure
│   ├── Integration
│   ├── Messaging
│   └── Services
│
└── MyProject.WebApi
    ├── Endpoints
    ├── Middlewares
    ├── Extensions
    └── Program.cs
```

---

## Layer Responsibilities

### In simple terms:
- **Domain** – the “how”: business rules, invariants, and behaviors
- **Application** – the “what”: use cases and orchestration of the flow
- **Infrastructure** – the “where and with what”: database, messaging, external services
- **Presentation (Web API)** – the entry point: translates HTTP into application calls

> This summary is especially helpful for those learning Clean Architecture for the first time.  
> — @Bruno Milagres

### Detailed

#### Domain
The **core of the system**.
- Business rules, invariants and behavior
- No dependencies on frameworks or infrastructure
- Fully testable in memory
Contains:
- Entities
- Value Objects
- Domain Exceptions
> The Domain answers **"how the business works"**.

---

#### Application
Defines **what the system does** through use cases.
- Orchestrates workflows
- Applies business rules via the Domain
- Depends only on abstractions
Key concepts:
- Vertical slices (`Features`)
- CQRS (Commands / Queries)
- Pipeline behaviors (validation, logging, transactions)
> The Application answers **"what needs to be done"**.

---

#### Infrastructure
Contains **technical implementations**.
- Databases, external services, messaging, integrations
- Implements interfaces defined in Application
- Knows about EF Core, HTTP, brokers, SDKs, etc.
> Infrastructure answers **"where and with what"**.

---

#### WebApi
The **entry point** of the system.
- Minimal APIs
- Endpoints only translate HTTP ↔ Application
- No business logic
Responsibilities:
- Request/response mapping
- Dependency Injection composition
- Middleware configuration
> WebApi is intentionally thin and replaceable.

---

## Why There Is No `Common` Project

This template **intentionally avoids a shared `Common` project**.

Reasons:
- `Common` tends to become a dumping ground
- Encourages tight coupling between layers
- Hides architectural boundaries

Instead:
- **Business concepts** live in Domain
- **Use case results / responses** live in Application
- **Technical helpers** live in Infrastructure

Each piece belongs **where it makes sense contextually**.

---

## Testing Strategy (Planned)

- Domain: pure unit tests
- Application: use case tests with mocked abstractions
- Infrastructure: integration tests
- WebApi: minimal smoke / contract tests

---

## Evolution Goals

This repository is expected to evolve with:
- Observability (OpenTelemetry)
- Persistence (EF Core)
- Messaging (outbox, async events)
- Authentication / Authorization
- Cloud readiness

Changes are intentional and documented as learning milestones.

---

## Usage

This repository is **not a NuGet package**.

It is meant to be:
- cloned
- adapted
- renamed
- evolved

Think of it as a **starting line**, not a finished product.

---

## Features Added

- **Serilog** for structured logging (all HTTP requests and application logs are output to the console)
- **OpenTelemetry** for distributed tracing (traces HTTP requests and exports to the console)
- **OpenAPI** documentation (JSON at `/openapi/v1.json`)
- **Scalar UI** for interactive API docs at `/`
- **Minimal health endpoint** at `/health` (`GET /health` returns `{ "status": "Healthy" }`)

### Main Packages
- [Serilog.AspNetCore](https://www.nuget.org/packages/Serilog.AspNetCore/): Logging
- [OpenTelemetry.Extensions.Hosting](https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting/): Tracing
- [Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi/): OpenAPI/Swagger
- [Scalar.AspNetCore](https://www.nuget.org/packages/Scalar.AspNetCore/): OpenAPI UI

---

## How to Run
1. Install the .NET 10 SDK
2. Restore packages:
   ```bash
   dotnet restore
   ```
3. Run the project:
   ```bash
   dotnet run --project MyProject.App/MyProject.Api.csproj
   ```
4. Access:
   - Scalar UI: [http://localhost:5000/](http://localhost:5000/) (or your configured port)
   - OpenAPI JSON: [http://localhost:5000/openapi/v1.json](http://localhost:5000/openapi/v1.json)
   - Health check: [http://localhost:5000/health](http://localhost:5000/health)

---

## Contribution
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
This project is licensed under the MIT License.

---

**Maintainer:** @Bruno Milagres – 2024-06-07

