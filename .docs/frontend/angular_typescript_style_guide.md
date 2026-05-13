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
