---
applyTo: "angular/**/*.{ts,html,css}"
---
<!-- Auto-generated for GitHub Copilot. Do not edit — update .docs/ sources and re-run build.ts. -->
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


# Angular & TypeScript Style Guide

Angular/TypeScript coding standards for this project. All developers and AI agents must follow these conventions.

## Naming

- **Files**: `kebab-case` matching the class name (e.g., `coffee.service.ts`, `home.component.ts`).
- **Components / Services / Classes**: `PascalCase`
- **Signals and local variables**: `camelCase`
- **Types and interfaces**: `PascalCase` — prefer `interface` for object shapes, `type` for unions.

## TypeScript

**Never use `any`** — use proper types, generics, or `unknown`:

```typescript
// ✅ Preferred
coffees = signal<Coffee[] | null>(null);

// ❌ Avoid
coffees = signal<any>(null);
```

**Use union types for constrained string values** rather than plain `string`:

```typescript
// ✅ Preferred
export type Roast = 'light' | 'medium' | 'dark';
type SortableKey = 'name' | 'origin' | 'body' | 'bitterness' | 'price';
```

## Components

**Use `inject()` over constructor injection:**

```typescript
// ✅ Preferred
export class HomeComponent {
  private svc = inject(CoffeeService);
}

// ❌ Avoid
export class HomeComponent {
  constructor(private svc: CoffeeService) {}
}
```

**Use signals for all reactive state:**

```typescript
coffees = signal<Coffee[] | null>(null);
roastFilter = signal<Roast | null>(null);

sortedCoffees = computed(() => {
  const roast = this.roastFilter();
  const coffees = this.coffees();
  if (!coffees) return null;
  return roast ? coffees.filter(c => c.roast === roast) : coffees;
});
```

## Services

- One service per resource (e.g., `CoffeeService` for all coffee-related endpoints).
- Return `Observable<T>` from all HTTP methods — do not `.subscribe()` inside a service.
- Define `API_BASE` as a module-level constant, not inline in each method.

```typescript
const API_BASE = 'http://localhost:5000';

@Injectable({ providedIn: 'root' })
export class CoffeeService {
  private http = inject(HttpClient);

  list(): Observable<Coffee[]> {
    return this.http.get<Coffee[]>(`${API_BASE}/api/coffees`);
  }
}
```

## Templates

- Use Bootstrap utility classes for layout and spacing — avoid inline styles.
- Prefer `*ngIf` with the `as` alias for null-safe rendering:
  ```html
  <div *ngIf="sortedCoffees() as list">...</div>
  ```
- Use the `currency` pipe for prices; prefer built-in Angular pipes over manual formatting in the component.


# Frontend Testing Style Guide

## Core Principles

- **Arrange / Act / Assert** – clear three-part structure per test.
- **Test behavior, not implementation** – assert what the user sees, not internal state or signal values directly.
- **No magic strings** – derive expected values from test data objects, not repeated literals.
- **Co-located tests** – place `.spec.ts` files next to the source file they test.

## Component Tests with TestBed

```typescript
describe('HomeComponent', () => {
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeComponent],
      providers: [
        { provide: CoffeeService, useValue: { list: () => of([]) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
  });

  it('displays a coffee card for each coffee returned by the service', () => {
    const coffees = [buildCoffee({ name: 'Espresso' })];
    fixture.componentInstance.coffees.set(coffees);
    fixture.detectChanges();

    const cards = fixture.nativeElement.querySelectorAll('.coffee-card');
    expect(cards.length).toBe(coffees.length);
    expect(cards[0].textContent).toContain(coffees[0].name);
  });
});
```

## Service Tests

- Test services directly without `TestBed` where possible — inject `HttpClient` using `HttpClientTestingModule`.
- Use `HttpTestingController` to assert the correct URL and HTTP method are called.

## What to Test

- **Components**: loading state, error state, rendered output for given data.
- **Services**: correct URL, HTTP method, and that the returned `Observable` emits the expected type.
- **Computed logic**: set signal values directly, assert `computed()` output — no DOM required.
- Do **not** test Angular framework behavior (routing wiring, DI resolution).

## Test Data

Prefer small builder functions over inline object literals to avoid duplication:

```typescript
const buildCoffee = (overrides: Partial<Coffee> = {}): Coffee => ({
  id: 'test-id',
  name: 'Test Coffee',
  origin: 'Test Origin',
  tastingNotes: '',
  bitterness: 5,
  acidity: 'medium',
  body: 3,
  roast: 'medium',
  bestFor: '',
  price: 10,
  ...overrides
});
```