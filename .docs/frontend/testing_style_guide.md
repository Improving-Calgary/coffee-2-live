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
