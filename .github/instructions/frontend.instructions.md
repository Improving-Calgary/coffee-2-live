---
applyTo: "blazor/**/*.{razor,cs}"
---
<!-- Auto-generated for GitHub Copilot. Do not edit — update .docs/ sources and re-run build.ts. -->
# Frontend Technical Design

Blazor WebAssembly app consuming the Coffee2Live API. Guides developers and AI agents for consistency and quality.

## Technology Stack

- **Framework**: Blazor WebAssembly (.NET)
- **Language**: C#
- **Styling**: Bootstrap 5 (utility classes)
- **HTTP**: `HttpClient` with `System.Net.Http.Json`
- **State**: Component fields + `StateHasChanged()`
- **Testing**: Playwright (E2E)

## Project Structure

- **`Models/`** – C# model classes (`Coffee`). No Blazor or HTTP dependencies.
- **`Services/`** – Plain C# classes that wrap `HttpClient` and return `Task<T?>`.
- **`Pages/`** – Route-level `.razor` components decorated with `@page`.
- **`Layout/`** – Shell components (`MainLayout`, `NavMenu`).
- **`_Imports.razor`** – Global `@using` directives for the entire project.
- **`Program.cs`** – Service registration and app startup.

## Component Conventions

- Use `@inject` to declare service dependencies at the top of each `.razor` file.
- Fetch data in `OnInitializedAsync`; store results in nullable private fields.
- Keep `@code` blocks focused on lifecycle and event handlers — no business logic.

## Data Flow

```
Template (@if / @foreach) → @code block → Service (Task<T?>) → API
```

- Services own the HTTP call and return `Task<T?>`.
- Components await the result in `OnInitializedAsync` and store it in a field.
- Templates render error → loading → content in that order.

## Architectural Constraints

- `Models/` must have zero Blazor or HTTP dependencies.
- Do not put business logic in services — keep filtering/sorting in the component.
- All service registrations go in `Program.cs`.
- `HttpClient.BaseAddress` is set once in `Program.cs` — do not hardcode API URLs in services or components.


# Blazor & C# Style Guide

Blazor/C# coding standards for this project. All developers and AI agents must follow these conventions.

## Naming

- **Files, Components, Services, Classes, Models**: `PascalCase` (e.g., `CoffeeService.cs`, `Home.razor`)
- **Local variables, parameters, private fields**: `camelCase` — no underscore prefix on fields

## Models

Initialize string properties to `string.Empty`; use `?` for values that may be absent:

```csharp
public class Coffee
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
}

private Coffee[]? coffees;
private string? error;
```

## Components (`.razor` files)

Use `@inject` for dependencies and `OnInitializedAsync` for async data fetching. Keep `@code` blocks focused on lifecycle and event handlers — no business logic:

```razor
@inject CoffeeService CoffeeService

@code {
    private Coffee[]? coffees;
    private string? error;

    protected override async Task OnInitializedAsync()
    {
        try { coffees = await CoffeeService.ListAsync(); }
        catch { error = "Failed to load coffees"; }
    }
}
```

## Services

- One service per resource.
- Use primary constructor injection; return `Task<T?>` from HTTP methods; use `GetFromJsonAsync<T>`.
- Register with `AddScoped` in `Program.cs` alongside an `HttpClient` pointing at the API base address.

```csharp
public class CoffeeService(HttpClient http)
{
    public Task<Coffee[]?> ListAsync() =>
        http.GetFromJsonAsync<Coffee[]>("/api/coffees");
}
```

## Templates

- Use Bootstrap utility classes — avoid inline styles.
- Always render error → loading → content in that order:

  ```razor
  @if (error is not null) { <div class="alert alert-danger">@error</div> }
  else if (coffees is null) { <div class="text-secondary">Loading&hellip;</div> }
  else { <!-- render list --> }
  ```

- Use `@foreach` for lists; use `<PageTitle>` on every page component.

## Global Imports

Place all `@using` directives in `_Imports.razor` — do not repeat them inside individual `.razor` files.


# Frontend Testing Style Guide

## Core Principles

- **Arrange / Act / Assert** – clear three-part structure per test.
- **Test behavior, not implementation** – assert what the user sees, not internal state.
- **No magic strings** – derive expected values from test data objects, not repeated literals.

## Tool

Tests use **Playwright** (`tests/coffees.spec.ts`). Run with `npx playwright test` from `blazor/tests/`.

The Blazor app is expected at `http://localhost:5177`. The .NET API does not need to be running — intercept the API route with `page.route()` and serve mock data.

## Test Structure

```typescript
test('renders coffees on the home page', async ({ page }) => {
  // Arrange — intercept API and return mock data
  await page.route('http://localhost:5000/api/coffees', async (route) => {
    await route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify([mockCoffee])
    });
  });

  // Act
  await page.goto('/');

  // Assert — verify rendered output
  await expect(page.locator('.coffee-card')).toHaveCount(1);
  await expect(page.getByRole('heading', { level: 5 }).filter({ hasText: mockCoffee.name })).toBeVisible();
});
```

## What to Test

- **Rendered output**: correct number of cards, correct content displayed per coffee.
- **Loading state**: element visible before the API responds.
- **Error state**: error message shown when the API returns a non-2xx response.
- Do **not** test Blazor framework internals (DI wiring, component lifecycle directly).

## Test Data

Use a shared inline object rather than repeating literals across tests:

```typescript
const mockCoffee = {
  id: '00000000-0000-0000-0000-000000000001',
  name: 'Ethiopian Yirgacheffe',
  origin: 'Ethiopia',
  tastingNotes: 'Floral, citrus, bergamot',
  roast: 'light',
  acidity: 'low',
  body: 3,
  bitterness: 2,
  bestFor: 'Pour-over',
  price: 18.99
};
```