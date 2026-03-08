
# ClaimsEngine

Healthcare claims processing engine (prototype). This repository contains core domain logic, application services, infrastructure adapters, hosts, and tests for processing and adjudicating healthcare claims.

## Overview

- Framework: .NET 8 (net8.0)
- Solution: `ClaimsEngine.sln`

This solution is organized to separate domain, application services, infrastructure, hosts, and tests.

## Projects

- `src/Core/ClaimsEngine.Domain` - domain model and value objects (no external dependencies)
- `src/Core/ClaimsEngine.Application` - application services, orchestrations (depends on Domain)
- `src/Infrastructure/ClaimsEngine.Infra.Data` - data adapters / repositories (depends on Application)
- `src/Infrastructure/ClaimsEngine.Infra.Messaging` - messaging adapters (depends on Application)
- `src/Hosts/ClaimsEngine.Ingestion.Api` - Web API host for claim ingestion (references Application)
- `src/Hosts/ClaimsEngine.Adjudication.Worker` - background worker for adjudication (references Application)
- `src/Hosts/ClaimsEngine.Ledger.Worker` - background worker for ledger tasks (references Application)

Tests:
- `tests/ClaimsEngine.Domain.UnitTests` - xUnit unit tests for Domain
- `tests/ClaimsEngine.Application.UnitTests` - xUnit unit tests for Application
- `tests/ClaimsEngine.IntegrationTests` - xUnit integration tests (references Application + infra)
- `tests/ClaimsEngine.LoadTests` - xUnit load tests (references Application)

## Getting started

Restore and build the solution:

```bash
dotnet restore ClaimsEngine.sln
dotnet build ClaimsEngine.sln -c Release
```

Run all tests:

```bash
dotnet test ClaimsEngine.sln
```

Run a host (example — API):

```bash
dotnet run --project src/Hosts/ClaimsEngine.Ingestion.Api -c Debug
```

Run a worker (example):

```bash
dotnet run --project src/Hosts/ClaimsEngine.Adjudication.Worker -c Debug
```

## Conventions

- Projects use prefix `ClaimsEngine.`
- Target framework: `net8.0`
- Use `dotnet` CLI for adding projects/references and running tests

## Next steps / TODO

- Add CI workflow to build and test on push
- Add README sections for coding guidelines and contributing

## License

Repository does not include a license file. Add one if you want this code to be reusable.# ClaimChain.Engine
High-throughput distributed claims adjudication and reconciliation engine. Built on .NET 9 and Azure Service Bus for event-driven medical billing workflows.
