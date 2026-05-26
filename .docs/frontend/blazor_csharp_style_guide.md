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
