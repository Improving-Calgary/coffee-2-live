# Coffee2Live – Agentic SDLC Feature Lab

> **Context for instructors:** Students have already completed the warm-up exercises (Tasks 2, 4, 5, 6). They have now received a lecture on Agentic SDLC, defined their team's development flow, and authored their workspace rules / instructions. This lab puts all of that into practice.
>
> **The pattern for every feature below is the same:**
> 1. **Spec** – use an AI skill to write a user story with acceptance criteria
> 2. **Design** – use an AI skill to produce a technical design / implementation plan
> 3. **Build** – hand the spec + design to the agent and let it implement end-to-end
> 4. **Review** – verify output against acceptance criteria; iterate if needed

---

## How to Work Through a Feature

Before opening any code file, do the following in your AI chat:

1. **Spec skill prompt** *(Ask or Code mode)*
   > *"Using the project rules in this workspace, write a user story and acceptance criteria for [feature name]."*

2. **Design skill prompt** *(Plan mode)*
   > *"Given the following user story and acceptance criteria, produce a step-by-step technical implementation plan scoped to this codebase."*
   Paste the output from step 1 into the prompt.

3. **Build prompt** *(Code mode or agent run)*
   > *"Implement the approved plan. Follow all workspace coding rules and standards."*

4. **Verify** – open the running app, run tests, check each acceptance criterion.

---

## Feature A – Coffee Favorites

### User Story
> As a coffee lover, I want to mark coffees as favorites so that I can quickly find the ones I enjoy most.

### Acceptance Criteria
- [ ] A heart / star toggle button appears on every coffee card
- [ ] Clicking the toggle marks or unmarks that coffee as a favorite (persisted in `localStorage`)
- [ ] A "Show favorites only" toggle at the top of the page filters the list to favorited coffees
- [ ] Favorite state survives a full page refresh
- [ ] No backend changes are required — this is purely client-side state

### Relevant Files
- `blazor/Pages/Home.razor` — add favorites state and toggle logic
- `blazor/Pages/Home.razor` — add toggle button and filter control (markup in same file)
- `blazor/wwwroot/css/app.css` — style the active/inactive favorite state
- `blazor/Models/Coffee.cs` — no change needed; favorites keyed by `coffee.Id`

### Instructor Notes
- Good test of **localStorage** integration via JS interop (`IJSRuntime`) with Blazor component state
- Students will see the agent inject `IJSRuntime` and call `localStorage.setItem` / `getItem` from C#
- Reinforce: the spec said "no backend changes" — the agent should respect that constraint

---

## Feature B – Coffee Search

### User Story
> As a coffee explorer, I want to search coffees by name, tasting notes, or origin so that I can quickly find a specific coffee without scrolling through the entire list.

### Acceptance Criteria
- [ ] A search input field appears above the coffee card grid
- [ ] Filtering is case-insensitive and matches partial strings
- [ ] The list updates live as the user types (no submit button required)
- [ ] When the search field is empty, all coffees are shown
- [ ] A clear (×) button appears inside the input when there is text, and resets the search on click
- [ ] If no results match, a friendly "No coffees found for '…'" message is displayed

### Relevant Files
- `blazor/Pages/Home.razor` — `searchTerm` field, filtered list in `@code` block
- `blazor/Pages/Home.razor` — search input with clear button (markup in same file)
- `blazor/wwwroot/css/app.css` — input and clear-button styling

### Instructor Notes
- Demonstrates composing multiple filter conditions (`searchTerm`, roast filter) in a single computed property
- Ask students: *"How do you prompt the agent to respect the existing filter logic and compose with it rather than replace it?"*

---

## Feature C – Coffee Detail Page with Ratings

### User Story
> As a coffee enthusiast, I want to view a dedicated detail page for each coffee, and leave a personal rating (1–5 stars), so that I can remember which coffees I liked.

### Acceptance Criteria
- [ ] Clicking a coffee card navigates to `/coffees/:id`
- [ ] The detail page displays all fields: name, origin, tasting notes, roast, acidity, body, bitterness, best for, and price
- [ ] A 1–5 star rating component is displayed; the user can click to set a rating
- [ ] The rating is saved to `localStorage` keyed by coffee `id`
- [ ] A "Back to list" link returns the user to the home page without a full reload
- [ ] The `GET /api/coffees/{id}` backend endpoint is used to load the detail data

### Relevant Files
- `blazor/App.razor` — routing is handled here; add `/coffees/{id}` route to the router
- `blazor/Services/CoffeeService.cs` — add `GetByIdAsync(Guid id)` method
- `blazor/Pages/` — new `CoffeeDetail.razor` page component (agent creates this)
- `dotnet/src/Coffee2Live.App/Controllers/CoffeesController.cs` — `GetById` already exists; verify it returns all fields

### Instructor Notes
- Full-stack feature: agent touches both Blazor and inspects (but likely does not modify) the .NET controller
- The star rating component is a good exercise in scoped component state vs. persisted state (JS interop for localStorage)
- Students should verify the agent adds the `@page` directive correctly and does not break the home route

---

## Feature D – Recommended Coffees Endpoint

### User Story
> As a developer, I want a `GET /api/coffees/recommended` endpoint that returns the top 3 coffees ranked by body score so that the frontend can display a "Top Picks" section without client-side sorting logic.

### Acceptance Criteria
- [ ] `GET /api/coffees/recommended` returns exactly 3 coffees
- [ ] The 3 coffees are those with the highest `Body` value; ties broken by `Name` ascending
- [ ] The endpoint returns the same `Coffee` shape as `GET /api/coffees`
- [ ] A unit test verifies the correct 3 coffees are returned from a known dataset
- [ ] The home page displays a "Top Picks" section above the main grid using this endpoint
- [ ] The "Top Picks" cards are visually distinct (e.g., a gold border or badge)

### Relevant Files
- `dotnet/src/Coffee2Live.App/Controllers/CoffeesController.cs` — add `GetRecommended()` action
- `dotnet/tests/Coffee2Live.Tests/CoffeesControllerTests.cs` — add test for recommendation logic
- `blazor/Services/CoffeeService.cs` — add `GetRecommendedAsync()` method
- `blazor/Pages/Home.razor` — load recommended coffees and render "Top Picks" section
- `blazor/wwwroot/css/app.css` — distinct card style

### Instructor Notes
- Spans both layers — agent must coordinate changes across .NET and Blazor
- Unit test requirement forces students to prompt the agent to write tests as part of the plan, not as an afterthought
- Discuss: *"How do you write workspace rules that make the agent always produce a test alongside production code?"*

---

## Feature E – "Add a New Coffee" Admin Form

### User Story
> As a coffee catalog administrator, I want a form to add a new coffee entry so that the catalog can be updated without editing JSON files manually.

### Acceptance Criteria
- [ ] A `/admin/add-coffee` route renders a form with all `Coffee` fields as inputs
- [ ] All fields are validated before submission (name required; bitterness 1–10; body 1–5; price ≥ 0)
- [ ] On submit, the form calls `POST /api/coffees` and displays a success or error message
- [ ] After a successful add, the form resets and the user can add another entry
- [ ] The backend `POST /api/coffees` endpoint persists the new coffee to `coffees.json`
- [ ] Duplicate names (case-insensitive) are rejected with a `409 Conflict` response
- [ ] A unit test covers the duplicate-name rejection logic

### Relevant Files
- `dotnet/src/Coffee2Live.App/Controllers/CoffeesController.cs` — add `POST` action, write to `coffees.json`
- `dotnet/src/Coffee2Live.Domain/Coffee.cs` — add Data Annotations attributes
- `blazor/Pages/` — new `AddCoffee.razor` page at `/admin/add-coffee`
- `blazor/Services/CoffeeService.cs` — add `AddAsync(Coffee coffee)` method
- `dotnet/tests/Coffee2Live.Tests/` — new test for duplicate rejection

### Instructor Notes
- Most complex feature in the lab — good capstone exercise
- Requires the agent to understand Blazor EditForm with DataAnnotations validation, HTTP POST with error handling, and .NET file I/O
- Ask students to write the acceptance criteria themselves first using the spec skill, then compare to the version here
- Highlight how precise AC ("409 Conflict for duplicate names") prevents the agent from inventing its own error behavior

---

## Debrief Questions

After completing any feature, reflect on these as a group:

1. **Spec quality:** Did the agent produce code that matched all acceptance criteria, or did vague criteria lead to unexpected behavior?
2. **Rule enforcement:** Which workspace rules did the agent follow automatically, and which did it ignore?
3. **Plan review:** Was the agent's implementation plan accurate before it wrote code? What would have gone wrong if you had skipped the plan step?
4. **Iteration:** How many rounds of correction were needed? What prompt changes reduced rework?
5. **Trust boundary:** What parts of the generated code did you feel you needed to review most carefully, and why?
