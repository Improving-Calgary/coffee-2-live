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
