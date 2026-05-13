# Coffee2Live – Hands-On AI Assistant Tasks

> Each task is designed to practice **inline completion** (tab/autocomplete in the editor) and/or **chat modes** (Ask, Code, or Plan). The existing three tasks are listed first for reference, followed by ten new tasks.

---

## Existing Tasks (Reference)

### Task 1 – Convert to Dark Mode
- Open the relevant CSS file (`angular/src/styles.css`)
- Construct a prompt to get dark mode
- Use **Chat / Edit**
- Dark Mode – Mission Accomplished

### Task 2 – Bug Report: "Coffee Origin is missing from page"
- Use **Chat (Ask mode)** to ask the codebase where `origin` may be getting lost
- Use **Autocomplete or Edit** when you find the bug to fix it

### Task 3 – Add API Health Check
- Go to `Program.cs` of the dotnet app
- Ask prompt to add health checks

---

## New Tasks

### Task 4 – Add a "Price" Field to the Coffee Model *(Full-Stack · Inline Completion + Chat)*

**Goal:** Show coffee price in card displayed in title as "Ethiopian Yirgacheffe ($12.99)".

**Steps:**
1. Open `dotnet/src/Coffee2Live.Domain/Coffee.cs`
2. Place your cursor after the `BestFor` property and use **inline completion** to add a `decimal Price` property
3. Open `dotnet/src/Coffee2Live.App/Controllers/CoffeesController.cs` and use **inline completion** in `LoadCoffees()` to map the new `Price` field from the DTO
4. Open `angular/src/app/models/coffee.ts` and use **inline completion** to add `price: number` to the interface
5. Open `angular/src/app/pages/home/home.component.html` and use **inline completion** to display the price as a badge inside the card
6. Open 'dotnet/src/Coffee2Live.App/Data/coffees.json' and use **inline mode or chat mode** to add a `price` field to each coffee

**Note:** As you move from file to file AI should understand and predict your next move.

**Modes to try:** Inline completion for each small addition, then **Ask mode** to verify nothing was missed.

---

### Task 5 – Write a Unit Test for `CoffeesController` *(Backend · Chat – Code Mode)*

**Goal:** Use AI to scaffold a meaningful unit test.

**Steps:**
1. Open `dotnet/tests/Coffee2Live.Tests/UnitTest1.cs`
2. Switch to **Code mode** and prompt: *"Generate a unit test that verifies `GetById` returns `NotFound` when an unknown GUID is provided"*
3. Review and accept the generated test
4. Use **inline completion** to add a second test case that asserts `GetAll` returns a non-null result

**Modes to try:** Code mode for initial scaffold, inline completion for the second assertion.

---

### Task 6 – Add Search / Filter by Roast Level *(Frontend · Chat – Plan Mode)*

**Goal:** Let users filter the coffee list by roast level without a page reload.

**Steps:**
1. Open **Plan mode** and describe the goal: *"Add a roast-level filter dropdown to the Coffee2Live home page that filters the displayed cards client-side"*
2. Review the plan the AI produces (component changes, template changes, no API changes needed)
3. Approve and let the AI apply changes to `home.component.ts` and `home.component.html`
4. Use **inline completion** to fine-tune the CSS for the dropdown in `home.component.css`

**Modes to try:** Plan mode to break down the work, inline completion for CSS tweaks.

---

### Task 7 – Add Sorting to the Coffee List *(Frontend · Inline Completion + Ask Mode)*

**Goal:** Sort coffees by name, bitterness, or body on the client side.

**Steps:**
1. Open `angular/src/app/pages/home/home.component.ts`
2. Use **inline completion** to add a `sortKey` signal and a `sortedCoffees` computed property
3. Open `home.component.html` and use **inline completion** to add a `<select>` bound to `sortKey`
4. If you get stuck, switch to **Ask mode** and ask: *"How should I wire an Angular signal-based computed to update the displayed list when sortKey changes?"*

**Modes to try:** Inline completion first, Ask mode as a reference when needed.

---

### Task 8 – Add a Coffee Detail Page *(Full-Stack · Chat – Code Mode)*

**Goal:** Clicking a coffee card navigates to a detail page showing all attributes.

**Steps:**
1. Use **Code mode** and prompt: *"Add a detail route `/coffees/:id` that calls `GET /api/coffees/{id}` and displays all Coffee fields"*
2. Review generated files: a new Angular component, a route entry in `app.routes.ts`, and a new method `getById()` in `coffee.service.ts`
3. Verify the backend `GET /api/coffees/{id}` endpoint already exists in `CoffeesController.cs`
4. Use **inline completion** to add a "Back" button to the detail page template

**Modes to try:** Code mode for scaffolding, inline completion for the back-navigation button.

---

### Task 9 – Add Request Logging Middleware *(Backend · Chat – Ask then Code Mode)*

**Goal:** Log every incoming HTTP request method and path to the console.

**Steps:**
1. Open `dotnet/src/Coffee2Live.App/Program.cs`
2. Use **Ask mode**: *"What is the simplest way to log every HTTP request in ASP.NET Core 8 using built-in middleware?"*
3. Switch to **Code mode** and prompt: *"Add a `app.Use` middleware lambda in Program.cs that logs `{method} {path}` before calling next"*
4. Use **inline completion** to add a timestamp to the log line

**Modes to try:** Ask mode for guidance, Code mode for implementation, inline completion for enhancement.

---

### Task 10 – Add Input Validation to the POST Endpoint *(Backend · Chat – Code Mode)*

**Goal:** Add a `POST /api/coffees` endpoint with Data Annotations validation.

**Steps:**
1. Open `CoffeesController.cs` and use **Code mode**: *"Add a POST endpoint that accepts a Coffee object, validates it using Data Annotations, and returns 400 Bad Request if invalid"*
2. Open `Coffee.cs` and use **inline completion** to add `[Required]` and `[Range]` attributes to the appropriate properties
3. Use **Ask mode** to confirm: *"Does ASP.NET Core automatically validate Data Annotations on `[ApiController]` endpoints?"*

**Modes to try:** Code mode to add the endpoint, inline completion for attributes, Ask mode to verify behavior.

---

### Task 11 – Generate an OpenAPI-Friendly Summary for Each Endpoint *(Backend · Inline Completion)*

**Goal:** Add `[EndpointSummary]` / XML doc comments so Swagger shows meaningful descriptions.

**Steps:**
1. Open `CoffeesController.cs`
2. Place your cursor above `GetAll()` and use **inline completion** to generate an XML `<summary>` doc comment
3. Repeat for `GetById()`
4. Open Coffee2Live.App.cspoj and use **inline completion** to add `<GenerateDocumentationFile>true</GenerateDocumentationFile>` to the project file
5. Open `Program.cs` and use **inline completion** to configure SwaggerGen to include XML comments. place cursor `builder.Services.AddSwaggerGen(` (the AI should generate the necessary code to read the XML file and include it in Swagger)
6. Run the API and open Swagger UI at `http://localhost:5000` to confirm the summaries appear
7. Use **Ask mode** if you need to enable XML documentation in the `.csproj`

**Modes to try:** Inline completion for doc comments, Ask mode for build configuration.

---

### Task 12 – Add an Angular Loading Skeleton *(Frontend · Chat – Code Mode)*

**Goal:** Replace the plain "Loading…" text with an animated skeleton card while data fetches.

**Steps:**
1. Open **Code mode** and prompt: *"Replace the loading state in `home.component.html` with three skeleton placeholder cards using only CSS animations – no external libraries"*
2. Accept the changes to `home.component.html` and `home.component.css`
3. Use **inline completion** in `home.component.css` to tweak the animation timing or colors to match the coffee brand palette in `styles.css`

**Modes to try:** Code mode for the skeleton markup and CSS, inline completion for brand-color tweaks.

---

### Task 13 – Add CORS Configuration for a Production Origin *(Backend · Ask + Inline Completion)*

**Goal:** Make the CORS policy configurable so it can support a deployed frontend URL.

**Steps:**
1. Open `Program.cs` and use **Ask mode**: *"How can I read the allowed CORS origins from `appsettings.json` instead of hardcoding them?"*
2. Using the AI's guidance, use **inline completion** to update the `AddCors` block to read from `builder.Configuration`
3. Add the corresponding key to `appsettings.json` (or create `appsettings.Development.json`) using **inline completion**

**Modes to try:** Ask mode for the pattern, inline completion for the implementation.

---

## Quick Reference – Chat Modes

| Mode | When to Use |
|------|-------------|
| **Ask** | Explore the codebase, understand patterns, get explanations |
| **Code** | Generate or refactor concrete code across one or more files |
| **Plan** | Break down a multi-step feature before writing any code |
| **Inline Completion** | Small additions—next line, next property, next method signature—without leaving the editor |
