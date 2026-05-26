<!-- Auto-generated for GitHub Copilot. Do not edit — update .docs/ sources and re-run build.ts. -->
# Product Overview

## What is Coffee2Live?

Coffee2Live is a full-stack web application for browsing and managing a curated coffee catalog.
## Core Problem Solved

Coffee enthusiasts want a simple way to discover and compare coffees without wading through unstructured information. Coffee2Live presents a structured catalog with consistent attributes — origin, tasting notes, roast, body, acidity, bitterness, and price — making it easy to find the right coffee for any occasion.

## Solution Approach

A lightweight .NET API serves coffee data from a JSON catalog, and a Blazor SPA consumes it to deliver a responsive, filterable card-based UI. There is no user authentication or database — the catalog is the product.


# Core Engineering Principles

All development should adhere to the following fundamental software engineering principles. These apply to both human and AI developers across the .NET backend and Blazor frontend.

- **Prefer Established Libraries**: Favor well-established libraries over custom implementations. Only implement custom solutions when no suitable library exists or specific requirements cannot be met.

- **Prefer Self-Documenting Code**: Use clear naming and descriptive identifiers; avoid redundant comments that restate what the code does. Comments should explain *why*, not *what*.

- **Single Responsibility Principle (SRP)**: Each class, function, or module should have one, and only one, reason to change. For example, a controller's responsibility is handling HTTP requests and responses — not sorting, filtering, or orchestrating complex logic.

- **Don't Repeat Yourself (DRY)**: Avoid duplicating code. If the same logic appears in multiple places, extract it into a shared method, service, or computed value.

- **Favor Composition Over Inheritance**: Build functionality through composition rather than inheritance hierarchies. Use inheritance sparingly and only for true "is-a" relationships.

- **Design for Testability**: Write code that is easy to test — favor dependency injection, keep constructors simple, and avoid logic that cannot be exercised in isolation.

- **Abstract External Integrations**: Place calls to external APIs behind a service abstraction (e.g., `CoffeeService` on the frontend) so they can be swapped or mocked independently of the components that consume them.

- **File Size and Responsibility Separation**: Split files that exceed ~300 lines or handle multiple distinct responsibilities. Each file should have a single, clear purpose.

- **Keep It Simple (KISS)**: Always prefer the simplest solution that solves the problem. Avoid unnecessary complexity or over-engineering.

- **You Ain't Gonna Need It (YAGNI)**: Do not implement functionality on the assumption it might be needed later. Implement only what is required to satisfy the current acceptance criteria.


# Definition of Done (DoD)

## Introduction

This defines the criteria that must be met for a user story to be considered "done". No story can be considered complete until all applicable items have been satisfied. This DoD applies to all user stories and should be used as a final quality gate by any developer, whether human or AI.

## The Checklist

A user story is considered **done** only when it meets all of the following criteria:

- [ ] **Code Complete**: All required code has been written to satisfy the user story's acceptance criteria.

- [ ] **Acceptance Criteria Met**: Every acceptance criterion in the user story has been implemented and verified.

- [ ] **Adheres to Technical Design**: The implementation complies with `.docs/backend/technical_design.md` and `.docs/frontend/technical_design.md`.

- [ ] **Tests Written and Passing**: Tests cover new business logic, edge cases, and error branches. All existing tests continue to pass.
    - Backend: `dotnet test` from `dotnet/` runs green.
    - Frontend: `npx playwright test` from `blazor/tests/` runs green.

- [ ] **No Build Warnings or Format Violations**: All code compiles cleanly and is correctly formatted.
    - Backend: `dotnet build` from `dotnet/` — zero warnings (enforced via `TreatWarningsAsErrors`).
    - Backend: `dotnet format --verify-no-changes` from `dotnet/` — exits clean.
    - Frontend: `dotnet build` from `blazor/` — zero warnings (enforced via `TreatWarningsAsErrors`).
    - Frontend: `dotnet format --verify-no-changes` from `blazor/` — exits clean.

- [ ] **Code Style**: The implementation follows the project style guides.
    - Backend: `.docs/backend/csharp_style_guide.md`
    - Frontend: `.docs/frontend/blazor_csharp_style_guide.md`

- [ ] **API Documentation Updated**: If any endpoints were added or modified, XML doc comments (`<summary>`, `<param>`, `<response>`) are present and the Swagger UI reflects the changes.

- [ ] **UI Verified**: If this is a UI story, the feature has been verified in a browser and behaves as described in the acceptance criteria.

- [ ] **Application Runs Successfully**: The full application starts without errors.
    - Backend: `dotnet run` from `dotnet/src/Coffee2Live.App/` — Swagger UI loads at `http://localhost:5000`.
    - Frontend: `dotnet run` from `blazor/` — app loads at `http://localhost:5177`.


# Code Review Guide

You are a code reviewer. Your goal is to catch real problems so the human reviewer can focus on high-level design decisions. The project's technical designs, engineering principles, and style guides are already available to you — read and apply them. Do the verification work yourself rather than just listing concerns.

## Review Priorities

In order of importance:

1. **Correctness** — Does the code satisfy the acceptance criteria? Are edge cases handled?
2. **Separation of Concerns** — Verify code is in the correct layer per `.docs/backend/technical_design.md` and `.docs/frontend/technical_design.md`. Flag layer violations as blockers (e.g., business logic in a controller, HTTP calls in a component instead of a service).
3. **Security** — No exposed secrets or hardcoded credentials, proper input validation, appropriate HTTP status codes.
4. **Test Coverage** — Are there tests for new business logic and error branches? Flag missing tests as blockers.
5. **Code Quality** — Follows engineering principles (SRP, DRY, KISS, YAGNI) and the relevant style guide.

## Severity

**Blockers** (must fix before merge):
- Code in the wrong architectural layer
- Missing or broken tests for changed functionality
- Security violations
- Breaking changes to the API contract without updating consumers

**Suggestions** (non-blocking):
- Opportunities to reduce duplication
- Missing edge case handling that doesn't affect core functionality
- Performance improvements for non-critical paths
- Include at most 3 suggestions per review, ranked by impact.

## Re-Review Policy

When a prior review comment already exists, the author has addressed feedback and pushed new changes:
- Report blockers only — omit suggestions entirely.
- Only flag something new if it is a genuine blocker introduced by the latest changes.
- Do not re-raise issues that have been fixed or acknowledged.

## What to Skip

Do not flag any of the following:
- Formatting or whitespace (style guides handle this)
- Naming preferences that already match existing project conventions
- Missing comments unless logic is truly unclear
- Generic advice like "consider adding more tests" without specifics

## Response Format

- Be concise — a few short bullet points per issue.
- For each issue, reference the specific file and line number.
- Explain *why* it's a problem, not just *what* the problem is.
- If there are no blockers, approve with a one-line summary. Do not hedge.
- Organize output: blockers first, then suggestions.