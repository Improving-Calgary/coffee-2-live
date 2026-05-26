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
