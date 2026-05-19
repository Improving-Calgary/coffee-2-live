# Angular → Blazor WebAssembly - Technical Migration Plan

## Executive Summary
**Migration Path**: Angular 20 (TypeScript SPA) → Blazor WebAssembly (.NET 8)  
**Migration Type**: Parallel addition — new `blazor/` folder added alongside existing `angular/`. Angular is not modified or removed. Both frontends target the same backend; student groups choose one frontend based on their familiarity and work exclusively within it.  
**Complexity**: Low — single phase, ~10 files, shared backend with no changes required.

---

## Current State Analysis

### Existing Tech Stack
- **Primary Technologies**: Angular 20, TypeScript 5.9, Node.js 18+
- **Dependencies**: `@angular/common`, `@angular/router`, `HttpClient`, `rxjs`, Bootstrap 5 (CDN), Playwright
- **Architecture Pattern**: Standalone SPA — single route (`/`) renders `HomeComponent`
- **Data Storage**: N/A (frontend only — reads from ASP.NET Core API)

### Current Architecture Assessment
The Angular app is a minimal, well-structured SPA:
- **One page**: `HomeComponent` at `/`
- **One service**: `CoffeeService` — `HttpClient.get<Coffee[]>('http://localhost:5000/api/coffees')`
- **One model**: `Coffee` interface with `id`, `name`, `origin`, `tastingNotes`, `bitterness`, `acidity`, `body`, `roast`, `bestFor`
- **UI features**: Coffee card grid (Bootstrap), loading state, error state
- **Styling**: Bootstrap 5 CDN + coffee-brown theme (`--coffee-accent: #6f4e37`)
- **Tests**: Playwright e2e — mocks API, verifies card rendering

The shared backend (`dotnet/`) is ASP.NET Core 8 at `http://localhost:5000`. It is **not in scope** for this migration.

### Migration Scope
**Included:**
- New `blazor/` folder with a standalone Blazor WebAssembly project
- Like-for-like implementation of all Angular app features
- Playwright e2e test equivalent

**Excluded:**
- `angular/` — untouched
- `dotnet/` — untouched (no CORS or port changes needed)

---

## Target State Definition

### Target Tech Stack
- **Primary Technologies**: Blazor WebAssembly (.NET 8), C# 12
- **Dependencies**: `Microsoft.AspNetCore.Components.WebAssembly` (8.x), Bootstrap 5 (CDN), `Microsoft.Playwright` (for e2e tests)
- **Architecture Pattern**: Standalone Blazor WASM SPA — single route (`/`) renders `Home.razor`
- **Data Storage**: N/A (frontend only — same API calls as Angular)

### Angular → Blazor Concept Mapping

| Angular | Blazor Equivalent |
|---|---|
| `Coffee` interface (`coffee.ts`) | `Coffee.cs` model class |
| `Acidity` / `Roast` union types (`'low'\|'medium'\|'high'`) | `int` fields — API serializes enums as integers |
| `CoffeeService` with `HttpClient` | `CoffeeService.cs` with typed `HttpClient` |
| `HomeComponent` class | `Home.razor` component |
| `*ngFor="let c of list"` | `@foreach (var c in list)` |
| `*ngIf="condition"` | `@if (condition)` |
| `signal<T>` / `computed()` | `private` C# fields + `StateHasChanged()` |
| `app.routes.ts` | `<Router>` in `App.razor` |
| Bootstrap CDN in `index.html` | Bootstrap CDN in `wwwroot/index.html` |
| `styles.css` | `wwwroot/css/app.css` |
| `playwright.config.ts` | `tests/playwright.config.ts` (Node.js tooling, same pattern) |

### Technical Benefits
- Groups familiar with Angular use `angular/`; groups familiar with Blazor use `blazor/` — no switching between frameworks during the workshop
- C# end-to-end — model types can conceptually map directly from the Domain project
- No new infrastructure — same backend, same port

### Success Criteria
- [ ] `blazor/` folder exists with a runnable Blazor WASM project
- [ ] `dotnet run` (in `blazor/`) serves the app and displays the coffee card grid
- [ ] Loading and error states display correctly
- [ ] Visual appearance matches Angular app (Bootstrap cards, coffee-brown theme)
- [ ] Playwright e2e test passes

---

## Migration Strategy

### Approach
**Selected Strategy**: Parallel addition using the Strangler Fig pattern (additive only).

**Rationale**: Angular must remain intact because different student groups will use it independently. The Blazor app is a new standalone project added to the repo. No shared code or build pipelines are modified. Each group works exclusively in their chosen frontend — there is no expectation of switching between or comparing Angular and Blazor during the workshop.

### Risk Assessment

#### Low Risks
- **Risk**: Bootstrap CDN version mismatch between Angular and Blazor
  - **Mitigation**: Use the same Bootstrap 5 CDN URL from the Angular `index.html`
- **Risk**: API response deserialization differences (camelCase JSON vs C# PascalCase)
  - **Mitigation**: Configure `JsonSerializerOptions` with `PropertyNameCaseInsensitive = true` in the Blazor HTTP client setup
- **Risk**: Students running both apps simultaneously causing port conflicts
  - **Monitoring**: Document in README that apps should be run one at a time

---

## Implementation Plan

### Phase 1: Scaffold & Configure the Blazor WASM Project
**Duration**: ~15 minutes

**Objectives**: Create the project structure and wire up the HTTP client to the existing backend.

**Key Activities**:

1. Scaffold the project from the repo root:
   ```bash
   dotnet new blazorwasm -o blazor --no-https
   ```

2. Clean out the default template files that will be replaced:
   - Delete `blazor/Pages/Counter.razor`
   - Delete `blazor/Pages/FetchData.razor`
   - Delete `blazor/Shared/SurveyPrompt.razor`
   - Clear the default content from `blazor/wwwroot/index.html` body (keep the structure)

3. Add Bootstrap 5 CDN to `blazor/wwwroot/index.html` `<head>`:
   ```html
   <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
   ```

4. Register the `HttpClient` pointing at the backend in `blazor/Program.cs`:
   ```csharp
   builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });
   ```

**Deliverables**: Runnable Blazor WASM project at `http://localhost:5177` (default) showing empty app shell.

---

### Phase 2: Implement the Coffee Model and Service
**Duration**: ~10 minutes

**Objectives**: Create the data model and HTTP service — equivalent to `coffee.ts` and `coffee.service.ts`.

**Key Activities**:

1. Create `blazor/Models/Coffee.cs`:
   ```csharp
   namespace Coffee2Live.Blazor.Models;

   public class Coffee
   {
       public Guid Id { get; set; }
       public string Name { get; set; } = string.Empty;
       public string Origin { get; set; } = string.Empty;
       public string TastingNotes { get; set; } = string.Empty;
       public int Bitterness { get; set; }
       public int Acidity { get; set; }
       public int Body { get; set; }
       public int Roast { get; set; }
       public string BestFor { get; set; } = string.Empty;
   }
   ```

2. Create `blazor/Services/CoffeeService.cs`:
   ```csharp
   using System.Net.Http.Json;
   using Coffee2Live.Blazor.Models;

   namespace Coffee2Live.Blazor.Services;

   public class CoffeeService(HttpClient http)
   {
       public Task<Coffee[]?> ListAsync() =>
           http.GetFromJsonAsync<Coffee[]>("/api/coffees");
   }
   ```

3. Register the service in `blazor/Program.cs`:
   ```csharp
   builder.Services.AddScoped<CoffeeService>();
   ```

**Deliverables**: `Coffee.cs` model and `CoffeeService.cs` registered and injectable.

---

### Phase 3: Implement the Home Page Component
**Duration**: ~20 minutes

**Objectives**: Build `Home.razor` — like-for-like equivalent of `HomeComponent`.

**Key Activities**:

1. Create `blazor/Pages/Home.razor` with the following structure:

   ```razor
   @page "/"
   @using Coffee2Live.Blazor.Models
   @using Coffee2Live.Blazor.Services
   @inject CoffeeService CoffeeService

   <div class="container py-4">
       <h1 class="display-5 fw-semibold mb-3">Coffee2Live</h1>
       <p class="text-secondary mb-4">Curated coffees you will love.</p>

       @if (error is not null)
       {
           <div class="alert alert-danger">@error</div>
       }
       else if (coffees is null)
       {
           <div class="text-secondary">Loading…</div>
       }
       else
       {
           <div class="row g-3">
               @foreach (var c in coffees!)
               {
                   <div class="col-12 col-md-6 col-lg-4">
                       <div class="card coffee-card h-100">
                           <div class="card-body">
                               <h5 class="card-title">@c.Name</h5>
                               <h6 class="card-subtitle mb-2 text-muted">@c.Origin</h6>
                               <p class="card-text small mb-2">@c.TastingNotes</p>
                               <div class="small">
                                   <span class="badge text-bg-light me-1">Roast: @c.Roast</span>
                                   <span class="badge text-bg-light me-1">Acidity: @c.Acidity</span>
                                   <span class="badge text-bg-light me-1">Body: @c.Body/5</span>
                                   <span class="badge text-bg-light me-1">Bitterness: @c.Bitterness/10</span>
                               </div>
                           </div>
                           <div class="card-footer text-muted small">Best for: @c.BestFor</div>
                       </div>
                   </div>
               }
           </div>
       }
   </div>

   @code {
       private Coffee[]? coffees;
       private string? error;

       protected override async Task OnInitializedAsync()
       {
           try
           {
               coffees = await CoffeeService.ListAsync();
           }
           catch
           {
               error = "Failed to load coffees";
           }
       }
   }
   ```

2. Add the coffee-brown theme to `blazor/wwwroot/css/app.css`:
   ```css
   :root {
     --coffee-bg: #f5efe6;
     --coffee-accent: #6f4e37;
     --coffee-accent-2: #a47148;
   }

   .container {
     background: var(--coffee-bg);
     border-radius: .5rem;
   }

   .coffee-card {
     border: 1px solid #e7dfd6;
   }

   .coffee-card .card-title {
     color: var(--coffee-accent);
   }

   .badge.text-bg-light {
     background-color: #f0e6dc !important;
     color: #4f3a2a;
   }
   ```

3. Update `blazor/Pages/Index.razor` (or `App.razor` routing) to point `/` to `Home`.

**Deliverables**: Fully functional home page displaying coffee cards with loading and error states.

---

### Phase 4: Add Playwright E2E Tests
**Duration**: ~15 minutes

**Objectives**: Add a Playwright test equivalent to `angular/tests/coffees.spec.ts`.

**Key Activities**:

1. Create `blazor/tests/` directory and initialize a Node.js test project:
   ```bash
   cd blazor/tests
   npm init -y
   npm install -D @playwright/test
   npx playwright install chromium
   ```

2. Create `blazor/tests/playwright.config.ts`:
   ```typescript
   import { defineConfig, devices } from '@playwright/test';

   export default defineConfig({
     testDir: '.',
     timeout: 30_000,
     expect: { timeout: 5_000 },
     fullyParallel: true,
     use: {
       baseURL: 'http://localhost:5177',
       trace: 'on-first-retry',
     },
     projects: [
       {
         name: 'chromium',
         use: { ...devices['Desktop Chrome'] },
       },
     ],
   });
   ```
   > Note: Update `baseURL` port to match whatever port `dotnet run` assigns.

3. Create `blazor/tests/coffees.spec.ts`:
   ```typescript
   import { test, expect } from '@playwright/test';

   test('renders coffees on the home page', async ({ page }) => {
     await page.route('http://localhost:5000/api/coffees', async (route) => {
       await route.fulfill({
         status: 200,
         contentType: 'application/json',
         body: JSON.stringify([
           {
             id: '00000000-0000-0000-0000-000000000001',
             name: 'Ethiopian Yirgacheffe',
             origin: 'Ethiopia',
             tastingNotes: 'Floral, citrus, bergamot',
             roast: 0,
             acidity: 0,
             body: 3,
             bitterness: 2,
             bestFor: 'Pour-over'
           },
           {
             id: '00000000-0000-0000-0000-000000000002',
             name: 'Colombian Supremo',
             origin: 'Colombia',
             tastingNotes: 'Chocolate, caramel, nutty',
             roast: 1,
             acidity: 1,
             body: 4,
             bitterness: 3,
             bestFor: 'Drip'
           }
         ])
       });
     });

     await page.goto('/');

     await expect(page.getByText('Coffee2Live')).toBeVisible();
     await expect(page.locator('.coffee-card')).toHaveCount(2);
     await expect(page.getByRole('heading', { level: 5 }).filter({ hasText: 'Ethiopian Yirgacheffe' })).toBeVisible();
     await expect(page.getByRole('heading', { level: 5 }).filter({ hasText: 'Colombian Supremo' })).toBeVisible();
   });
   ```

4. Add an `e2e` script to `blazor/` — either in the `.csproj` as a `dotnet` task or a small `package.json` at the project root for convenience.

**Deliverables**: `coffees.spec.ts` passing against the running Blazor app.

---

### Post-Migration
**Duration**: ~5 minutes

**Objectives**: Verify parity and update repo documentation.

**Key Activities**:
1. Run the Blazor app independently and confirm identical UI to Angular
2. Update repo `README.md` to document the `blazor/` project and how to run it
3. Note: students will never need to run both apps simultaneously — each group uses one frontend exclusively throughout the workshop

---

## Testing & Validation Strategy

### Testing Approach
- **E2E Testing**: Playwright test mocks the API and validates card rendering — no backend required to run tests
- **Manual Validation**: Run the Blazor app independently and verify it matches the expected UI (reference the Angular app or screenshot as a guide, not as a live running comparison)

### Technical Validation Checkpoints
- [ ] `dotnet run` in `blazor/` starts without errors
- [ ] Home page loads and shows coffee cards from the live API
- [ ] Loading spinner appears briefly before data loads
- [ ] Error message appears when API is unreachable (stop the backend to test)
- [ ] Playwright test passes: `npx playwright test` in `blazor/tests/`
- [ ] Visual parity with Angular app confirmed (card layout, colors, badges)

---

## Resource Requirements

### Team Composition
- **1 Developer / Instructor**: ~1 hour total to implement all phases
- **Students**: Each group works exclusively in their chosen frontend (`angular/` or `blazor/`). Phases 2–4 are hands-on exercises; Phase 1 scaffold can be pre-done by the instructor.

### Infrastructure Requirements
- .NET 8 SDK (already required for the backend)
- Node.js 18+ (already required for Angular)
- No new cloud services or infrastructure

---

## Dependencies & Prerequisites

### Technical Dependencies
- .NET 8 SDK installed (`dotnet --version` ≥ 8.0)
- `dotnet new blazorwasm` template available (included with .NET 8 SDK)
- Node.js + npm (for Playwright tests)
- Backend running at `http://localhost:5000` for manual validation

### Operational Dependencies
- None — this is purely additive to the repo

---

## Contingency Planning

### Rollback Procedures
- **Full rollback**: Delete the `blazor/` folder. Angular and the backend are completely unaffected.
- **Partial rollback**: Any phase can be reverted independently since phases build on each other additively.

---

## Migration Execution Notes

### Code Transformation Patterns

| Angular Pattern | Blazor Pattern |
|---|---|
| `signal<T \| null>(null)` | `private T? field;` |
| `this.svc.list().subscribe({ next, error })` | `await CoffeeService.ListAsync()` in `OnInitializedAsync` |
| `*ngFor="let c of list"` | `@foreach (var c in list) { ... }` |
| `*ngIf="x"` | `@if (x) { ... }` |

### Common Pitfalls & Solutions
- **JSON deserialization**: The API returns camelCase JSON (`"tastingNotes"`), but C# properties are PascalCase. Fix: `GetFromJsonAsync` handles this automatically via `JsonSerializerDefaults.Web`.
- **Enum serialization**: The API serializes enums as integers (e.g. `Roast.Light` → `0`). The Blazor `Coffee` model uses `int` for `Roast` and `Acidity` to match — do **not** use C# enums here.
- **Port number**: Blazor WASM dev server assigns a random port. The CORS policy on the backend uses `SetIsOriginAllowed` to allow any localhost port.
- **Bootstrap not loading**: Ensure the Bootstrap CDN `<link>` is in `wwwroot/index.html`, not in a `.razor` file.

### Step-by-Step Execution Guide
1. `dotnet new blazorwasm -o blazor --no-https` from repo root
2. Delete template boilerplate files (Counter, FetchData, SurveyPrompt)
3. Add Bootstrap 5 CDN to `wwwroot/index.html`
4. Create `Models/Coffee.cs`
5. Create `Services/CoffeeService.cs`
6. Register `HttpClient` and `CoffeeService` in `Program.cs`
7. Create `Pages/Home.razor`
8. Add coffee-brown CSS to `wwwroot/css/app.css`
9. Run `dotnet run` and verify app at `http://localhost:5177`
10. Add Playwright tests in `tests/` and run `npx playwright test`
11. Update repo `README.md`

---

*This migration plan was generated with the help of the Marco Solution Architect persona.*
