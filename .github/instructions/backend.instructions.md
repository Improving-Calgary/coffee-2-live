---
applyTo: "dotnet/**/*.cs"
---
<!-- Auto-generated for GitHub Copilot. Do not edit — update .docs/ sources and re-run build.ts. -->
# Backend Technical Design

ASP.NET Core Web API serving the Coffee2Live catalog. Guides developers and AI agents for consistency and quality.

## Technology Stack

- **Framework**: ASP.NET Core (Minimal Hosting Model)
- **Language**: C# (.NET 8)
- **Serialization**: `System.Text.Json` with camelCase enum conversion
- **API Docs**: Swagger / OpenAPI (XML comments)
- **Testing**: NUnit + FluentAssertions

## Project Structure

Three projects with clear separation of concerns:

- **`Coffee2Live.Domain`** – Domain layer: plain C# models and enums (`Coffee`, `Acidity`, `Roast`). Zero external dependencies.
- **`Coffee2Live.App`** – API layer: ASP.NET Core controllers, `Program.cs` startup, and the `Data/coffees.json` data file.
- **`Coffee2Live.Recommendation`** – Domain service layer: business logic that does not belong in controllers (e.g., ranking, filtering).
- **`Coffee2Live.Tests`** – Test project: NUnit tests for controller and domain logic.

## API Conventions

- All routes follow the pattern `api/[controller]` (attribute routing on `ControllerBase`).
- Controllers must be thin: load data, delegate any logic to a service or LINQ query, return an `ActionResult<T>`.
- Use `Ok()`, `NotFound()`, `BadRequest()`, and `Conflict()` — do not return raw objects.
- Enums are serialized as camelCase strings (configured globally in `Program.cs`).
- All public controller actions require XML doc comments (`<summary>`, `<param>`, `<response>`).

## Data Access

- Coffee data is stored in `dotnet/src/Coffee2Live.App/Data/coffees.json`.
- `CoffeesController` loads this file via `IWebHostEnvironment.ContentRootPath` and deserializes it into domain models.
- IDs are deterministic GUIDs derived from the coffee name (MD5-based); do not change this scheme without updating tests.
- For write operations (POST), persist changes back to the same `coffees.json` file.

## Dependency Injection

- Register new services in `Program.cs` using `builder.Services`.
- Prefer constructor injection; avoid service locator / `IServiceProvider` calls in application code.

## Architectural Constraints

- `Coffee2Live.Domain` must not reference `Coffee2Live.App` or any ASP.NET packages.
- Controllers must not contain business logic beyond request parsing and response shaping.
- Do not introduce a database — `coffees.json` is the intentional data store for this sample project.


# C# Style Guide

C# coding standards for this project. All developers and AI agents must follow these conventions.

## Naming

- **Classes / interfaces / enums**: `PascalCase`
- **Methods and properties**: `PascalCase`
- **Local variables and parameters**: `camelCase`
- **Private fields**: `_camelCase` (underscore prefix)
- **Constants**: `PascalCase` (not `ALL_CAPS`)

## Type Declarations

**Use `var` when the type is obvious from the right-hand side:**

```csharp
// ✅ Preferred
var coffees = new List<Coffee>();
var name = coffee.Name;

// ❌ Avoid
List<Coffee> coffees = new List<Coffee>();
string name = coffee.Name;
```

**Use target-typed `new` for constructor calls:**

```csharp
// ✅ Preferred
List<Coffee> coffees = new();

// ❌ Avoid
List<Coffee> coffees = new List<Coffee>();
```

**Use file-scoped namespaces:**

```csharp
// ✅ Preferred
namespace Coffee2Live.Domain;

public class Coffee { }

// ❌ Avoid
namespace Coffee2Live.Domain
{
    public class Coffee { }
}
```

## Null Handling

**Use null-coalescing and null-conditional operators:**

```csharp
// ✅ Preferred
var name = coffee?.Name ?? string.Empty;

// ❌ Avoid
var name = coffee != null ? coffee.Name : string.Empty;
```

**Use `string.IsNullOrWhiteSpace` rather than manual null/empty checks.**

## LINQ

- Prefer method syntax (`.Where()`, `.Select()`, `.OrderBy()`) over query syntax.
- Keep LINQ chains readable — one operator per line for chains longer than two steps.

```csharp
// ✅ Preferred
var topPicks = coffees
    .OrderByDescending(c => c.Body)
    .ThenBy(c => c.Name)
    .Take(3)
    .ToList();
```


# Backend Testing Style Guide

## Core Principles

- **Arrange / Act / Assert** – clear three-part structure with a blank line between sections.
- **One concept per test** – multiple `Should()` calls are fine when verifying the same outcome.
- **No magic strings** – derive expected values from the objects under test, not hardcoded literals.
- **Descriptive names** – `Method_Condition_ExpectedBehavior` (e.g., `GetById_ReturnsNotFound_WhenIdDoesNotExist`).
- **Group by class** – one file per class under test (e.g., `CoffeesControllerTests.cs`); use `// --- MethodName ---` comments to separate groups.
- **Fakes over mocks** – use hand-written `sealed` fake classes (e.g., `FakeWebHostEnvironment`), named with a `Fake` prefix, kept at the bottom of the test file.
- **FluentAssertions only** – never use `Assert.AreEqual` or other NUnit-native assertions.

## Writing Tests

```csharp
[Test]
public void GetAll_ReturnsOk_WithCoffees_WhenFileExists()
{
    WriteJson("""[{ "name": "Espresso", ... }]""");

    var result = CreateController().GetAll();

    var ok = result.Result as OkObjectResult;
    ok.Should().NotBeNull();
    (ok!.Value as IEnumerable<Coffee>).Should().HaveCount(1);
}
```

Derive IDs and values from the SUT — never hardcode them:

```csharp
// ✅ Preferred
var expectedId = GetAllCoffees().First().Id;
controller.GetById(expectedId).Result.Should().BeOfType<OkObjectResult>();

// ❌ Avoid
controller.GetById(Guid.Parse("a1b2c3d4-..."));
```

## What to Test

- **Controller actions**: happy path and all error branches (404, 409, 400).
- **Recommendation logic**: any method in `Coffee2Live.Recommendation` that transforms or filters data.
- **Edge cases**: empty file, missing file, null fields, unrecognized enum values.
- Do **not** test `Program.cs` startup or infrastructure wiring.