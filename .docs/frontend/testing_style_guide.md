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
