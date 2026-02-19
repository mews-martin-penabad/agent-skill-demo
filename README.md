# SkillsWorkshop — Agent Skill Demo

A .NET 10 Clean Architecture Web API for card payments, used as a live demo for building and using **Cursor Agent Skills**.

---

## What is an Agent Skill?

A **Cursor Agent Skill** is a Markdown file that lives in `.skills/<skill-name>/SKILL.md` inside your repository. When you invoke it in Cursor, the AI agent reads the skill instructions before generating any code — so every output matches your team's conventions automatically.

This repo ships one skill:

```
.skills/
└── dotnet-boilerplate-generator/
    └── SKILL.md
```

The `dotnet-boilerplate-generator` skill knows:

- The Clean Architecture layer structure of this solution
- Naming conventions for controllers, services, repositories, and DTOs
- That all methods must be async with `CancellationToken`
- That domain models must never be exposed directly — DTOs only
- Exactly which files to generate and where to place them

Instead of writing boilerplate by hand or copying from another entity, you describe what you want and the agent produces the full stack in one shot.

---

## Solution Structure

```
SkillsWorkshop/
├── SkillsWorkshop.API
│   ├── Controllers/CardPaymentController.cs
│   └── Program.cs
├── SkillsWorkshop.Application
│   ├── DTOs/CardPaymentDto.cs
│   ├── Interfaces/ICardPaymentRepository.cs
│   ├── Interfaces/ICardPaymentService.cs
│   └── Services/CardPaymentService.cs
├── SkillsWorkshop.Domain
│   └── CardPayment.cs
├── SkillsWorkshop.Infrastructure
│   ├── Data/AppDbContext.cs
│   └── Repositories/CardPaymentRepository.cs
└── SkillsWorkshop.Tests
    └── CardPaymentServiceTests.cs
```

**Dependency direction:** `API → Application ← Infrastructure`, `Application → Domain`

---

## Running the API

```bash
dotnet run --project SkillsWorkshop.API
```

The Scalar UI is available at `http://localhost:<port>/scalar/v1` when running in Development.

---

## Live Demo — The Contrast

The most effective way to show the value of a skill is to run the same prompt twice: once without the skill, once with.

### Step 1 — Without the skill

Open a fresh Cursor Agent chat and type:

```
Add a Refund entity to this .NET API. A refund has an OriginalPaymentId, an Amount,
a Reason, and a RefundStatus (Pending, Processed, Rejected).
```

The AI will produce something functional but will drift from your conventions. Watch for:

- Wrong or missing namespaces
- `CancellationToken` dropped from method signatures
- `IActionResult` instead of `ActionResult<T>`
- No test file generated
- Files placed in the wrong folders

Discard the changes without applying them.

### Step 2 — With the skill

Open a fresh chat, reference the skill file first, then use the same prompt:

```
@.skills/dotnet-boilerplate-generator/SKILL.md

Add a Refund entity to this .NET API. A refund has an OriginalPaymentId, an Amount,
a Reason, and a RefundStatus (Pending, Processed, Rejected).
```

The agent reads the skill, reads the `CardPayment` reference implementation, and produces output that matches the codebase exactly — correct namespaces, correct patterns, tests included, wiring steps provided.

The skill isn't magic. It's structured context that every developer on the team gets for free, every time.

---

## Using the Skill — Walkthrough

### Invoking the skill

In Cursor's Agent chat, reference the skill file and describe what you want:

```
@.skills/dotnet-boilerplate-generator/SKILL.md

Generate boilerplate for a `Refund` entity. A refund has an `OriginalPaymentId`,
an `Amount`, a `Reason`, and a `RefundStatus` (Pending, Processed, Rejected).
```

The agent will generate all 8 files fully wired and ready to drop in:

| File | Layer |
|---|---|
| `SkillsWorkshop.Domain/Refund.cs` | Domain |
| `SkillsWorkshop.Application/DTOs/RefundDto.cs` | Application |
| `SkillsWorkshop.Application/Interfaces/IRefundRepository.cs` | Application |
| `SkillsWorkshop.Application/Interfaces/IRefundService.cs` | Application |
| `SkillsWorkshop.Application/Services/RefundService.cs` | Application |
| `SkillsWorkshop.Infrastructure/Repositories/RefundRepository.cs` | Infrastructure |
| `SkillsWorkshop.API/Controllers/RefundController.cs` | API |
| `SkillsWorkshop.Tests/RefundServiceTests.cs` | Tests |

After applying the files, add the two wiring steps:

**`AppDbContext.cs`:**
```csharp
public DbSet<Refund> Refunds => Set<Refund>();
```

**`Program.cs`:**
```csharp
builder.Services.AddScoped<IRefundService, RefundService>();
builder.Services.AddScoped<IRefundRepository, RefundRepository>();
```

---

## Tech Stack

- .NET 10 Web API
- Entity Framework Core (InMemory — no setup needed)
- Scalar for API docs (`/scalar/v1`)
- xUnit + Moq + FluentAssertions for tests

## Running Tests

```bash
dotnet test
```
