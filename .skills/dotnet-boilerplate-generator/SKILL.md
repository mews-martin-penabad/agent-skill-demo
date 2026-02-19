---

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

### Namespaces
- `SkillsWorkshop.Domain`
- `SkillsWorkshop.Application.DTOs`
- `SkillsWorkshop.Application.Interfaces`
- `SkillsWorkshop.Application.Services`
- `SkillsWorkshop.Infrastructure.Repositories`
- `SkillsWorkshop.API.Controllers`
- `SkillsWorkshop.Tests`

### Tech Stack
- .NET 10 Web API
- Entity Framework Core for data access
- Dependency Injection via built-in Microsoft.Extensions.DependencyInjection
- xUnit + Moq + FluentAssertions for unit tests

### Patterns
- Async/await throughout (all service and repository methods must be async)
- Constructor injection only — no service locator
- Return `ActionResult<T>` from controllers
- Use `CancellationToken` in all async method signatures

## Reference Implementation

Use the existing `CardPayment` entity as the reference for style and structure:

- `SkillsWorkshop.Domain/CardPayment.cs`
- `SkillsWorkshop.Application/DTOs/CardPaymentDto.cs`
- `SkillsWorkshop.Application/Interfaces/ICardPaymentRepository.cs`
- `SkillsWorkshop.Application/Interfaces/ICardPaymentService.cs`
- `SkillsWorkshop.Application/Services/CardPaymentService.cs`
- `SkillsWorkshop.Infrastructure/Repositories/CardPaymentRepository.cs`
- `SkillsWorkshop.API/Controllers/CardPaymentController.cs`
- `SkillsWorkshop.Tests/CardPaymentServiceTests.cs`

Read these files before generating output. Match their imports, namespaces, patterns, and style exactly.

## Output Instructions

When generating boilerplate for a given entity (e.g. `Refund`):

1. **Domain Model** — `SkillsWorkshop.Domain/{Entity}.cs` — simple POCO with Id and relevant properties
2. **DTOs** — `SkillsWorkshop.Application/DTOs/{Entity}Dto.cs` — `{Entity}Dto`, `Create{Entity}Dto`, `Update{Entity}Dto`
3. **Repository Interface** — `SkillsWorkshop.Application/Interfaces/I{Entity}Repository.cs` — CRUD methods, async
4. **Service Interface** — `SkillsWorkshop.Application/Interfaces/I{Entity}Service.cs` — mirrors repository but takes/returns DTOs
5. **Service Implementation** — `SkillsWorkshop.Application/Services/{Entity}Service.cs` — maps between domain and DTOs
6. **Repository Implementation** — `SkillsWorkshop.Infrastructure/Repositories/{Entity}Repository.cs` — EF Core implementation
7. **Controller** — `SkillsWorkshop.API/Controllers/{Entity}Controller.cs` — RESTful endpoints: GET all, GET by id, POST, PUT, DELETE
8. **Unit Tests** — `SkillsWorkshop.Tests/{Entity}ServiceTests.cs` — xUnit + Moq + FluentAssertions, testing all service methods

Generate all files in one response, clearly labelled with file paths.

After generating the files, apply the two wiring changes:

**`SkillsWorkshop.Infrastructure/Data/AppDbContext.cs`** — add the DbSet after the existing ones:
```csharp
public DbSet<{Entity}> {Entity}s => Set<{Entity}>();
```

**`SkillsWorkshop.API/Program.cs`** — add the DI registrations before `var app = builder.Build();`:
```csharp
builder.Services.AddScoped<I{Entity}Service, {Entity}Service>();
builder.Services.AddScoped<I{Entity}Repository, {Entity}Repository>();
```

## Example

If the user says: *"Generate boilerplate for a Refund entity"*, produce:

- `SkillsWorkshop.Domain/Refund.cs`
- `SkillsWorkshop.Application/DTOs/RefundDto.cs`
- `SkillsWorkshop.Application/Interfaces/IRefundRepository.cs`
- `SkillsWorkshop.Application/Interfaces/IRefundService.cs`
- `SkillsWorkshop.Application/Services/RefundService.cs`
- `SkillsWorkshop.Infrastructure/Repositories/RefundRepository.cs`
- `SkillsWorkshop.API/Controllers/RefundController.cs`
- `SkillsWorkshop.Tests/RefundServiceTests.cs`

Then apply the wiring changes to `AppDbContext.cs` and `Program.cs`.
