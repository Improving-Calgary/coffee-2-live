# Frontend Technical Design

Angular SPA consuming the Coffee2Live API. Guides developers and AI agents for consistency and quality.

## Technology Stack

- **Framework**: Angular 19 (standalone components)
- **Language**: TypeScript
- **Styling**: Bootstrap 5 (utility classes)
- **HTTP**: `HttpClient` via `@angular/common/http`
- **State**: Angular Signals
- **Testing**: Jasmine + Angular `TestBed`

## Project Structure

- **`src/app/models/`** – TypeScript interfaces and types (`Coffee`, `Roast`, `Acidity`). No Angular dependencies.
- **`src/app/services/`** – Injectable services that wrap `HttpClient` and return `Observable<T>`.
- **`src/app/pages/`** – Route-level components (one folder per page, e.g., `home/`).
- **`src/app/app.routes.ts`** – All route definitions; add new routes here.
- **`src/app/app.config.ts`** – Application providers (`provideRouter`, `provideHttpClient`, etc.).

## Component Conventions

- All components are **standalone** (`standalone: true` in `@Component`).
- Use `inject()` instead of constructor injection for services.
- Use **signals** (`signal()`, `computed()`) for component state — avoid `BehaviorSubject` or plain class fields for reactive state.
- Keep components thin: data fetching in the `constructor` or `ngOnInit` via a service, logic in `computed()`, display in the template.

## Data Flow

```
Template → Component (signals + computed) → Service (Observable) → API
```

- Services own the HTTP call and return `Observable<T>`.
- Components subscribe once (in constructor) and store results in a signal.
- Templates read signals via `signal()` calls; use `*ngIf` / `*ngFor` for conditional rendering.

## Architectural Constraints

- `models/` must have zero Angular or HTTP dependencies.
- Do not put business logic (sorting, filtering) in services — keep it in `computed()` inside the component.
- All new providers go in `app.config.ts`, not in individual components.
- `API_BASE` URL is defined as a constant in the service file — do not hardcode it in components.
