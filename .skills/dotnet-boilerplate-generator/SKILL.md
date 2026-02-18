
--
name: dotnet-boilerplate-generator
description: >
Generates consistent .NET boilerplate code including controllers, services,
repositories, and DTOs. Use when creating new features, entities, or API
endpoints following the team's conventions.
---

# .NET Boilerplate Generator

## When to use this skill
Use this skill when the user asks to:
- Create a new API endpoint or controller
- Scaffold a new feature or entity
- Generate a service, repository, or DTO class
- Set up the full CRUD stack for a given entity

## Project Conventions

### Architecture
- Clean Architecture: API → Application → Domain → Infrastructure
- Repository pattern for data access
- Service layer for business logic
- DTOs for all API input and output (never expose domain models directly)

### Naming
- Controllers: `{Entity}Controller`
- Services: `I{Entity}Service` interface + `{Entity}Service` implementation
- Repositories: `I{Entity}Repository` interface + `{Entity}Repository` implementation
- DTOs: `{Entity}Dto`, `Create{Entity}Dto`, `Update{Entity}Dto`

### Tech Stack
- .NET 8 Web API
- Entity Framework Core for data access
- Dependency Injection via built-in Microsoft.Extensions.DependencyInjection
- xUnit + Moq for unit tests

### Patterns
- Async/await throughout (all service and repository methods must be async)
- Constructor injection only — no service locator
- Return `IActionResult` or `ActionResult<T>` from controllers
- Use `CancellationToken` in all async method signatures

## Output Instructions

When generating boilerplate for a given entity (e.g. `Product`):

1. **Domain Model** — simple POCO with Id and relevant properties
2. **DTOs** — `{Entity}Dto`, `Create{Entity}Dto`, `Update{Entity}Dto`
3. **Repository Interface + Implementation** — CRUD methods, async
4. **Service Interface + Implementation** — business logic layer wrapping the repository
5. **Controller** — RESTful endpoints: GET all, GET by id, POST, PUT, DELETE

Generate all files in one response, clearly labelled with file paths.

## Example

If the user says: *"Generate boilerplate for a CardPayment entity"*, produce:

- `Domain/CardPayment.cs`
- `Application/DTOs/CardPaymentDto.cs`
- `Application/Interfaces/ICardPaymentRepository.cs`
- `Infrastructure/Repositories/CardPaymentRepository.cs`
- `Application/Interfaces/ICardPaymentService.cs`
- `Application/Services/CardPaymentService.cs`
- `API/Controllers/CardPaymentController.cs`