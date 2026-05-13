# Definition of Done (DoD)

## Introduction

This defines the criteria that must be met for a user story to be considered "done". No story can be considered complete until all applicable items have been satisfied. This DoD applies to all user stories and should be used as a final quality gate by any developer, whether human or AI.

## The Checklist

A user story is considered **done** only when it meets all of the following criteria:

- [ ] **Code Complete**: All required code has been written to satisfy the user story's acceptance criteria.

- [ ] **Acceptance Criteria Met**: Every acceptance criterion in the user story has been implemented and verified.

- [ ] **Adheres to Technical Design**: The implementation complies with `.docs/backend/technical_design.md` and `.docs/frontend/technical_design.md`.

- [ ] **Tests Written and Passing**: Unit tests cover new business logic, edge cases, and error branches. All existing tests continue to pass.
    - Backend: `dotnet test` from `dotnet/` runs green.
    - Frontend: `ng test --watch=false` from `angular/` runs green.

- [ ] **Code Style**: The implementation follows the project style guides.
    - Backend: `.docs/backend/csharp_style_guide.md`
    - Frontend: `.docs/frontend/angular_style_guide.md`

- [ ] **API Documentation Updated**: If any endpoints were added or modified, XML doc comments (`<summary>`, `<param>`, `<response>`) are present and the Swagger UI reflects the changes.

- [ ] **UI Verified**: If this is a UI story, the feature has been verified in a browser and behaves as described in the acceptance criteria.

- [ ] **Application Runs Successfully**: The full application starts without errors.
    - Backend: `dotnet run` from `dotnet/src/Coffee2Live.App/` — Swagger UI loads at `http://localhost:5000`.
    - Frontend: `ng serve` from `angular/` — app loads at `http://localhost:4200`.
