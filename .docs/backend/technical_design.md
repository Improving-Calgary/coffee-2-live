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
