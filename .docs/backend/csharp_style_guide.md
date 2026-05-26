# C# Style Guide

C# coding standards for this project. All developers and AI agents must follow these conventions.

## Naming

- **Classes / interfaces / enums**: `PascalCase`
- **Methods and properties**: `PascalCase`
- **Local variables and parameters**: `camelCase`
- **Private fields**: `_camelCase` (underscore prefix)
- **Constants**: `PascalCase` (not `ALL_CAPS`)

## Type Declarations

**Use `var` when the type is obvious from the right-hand side:**

```csharp
// ✅ Preferred
var coffees = new List<Coffee>();
var name = coffee.Name;

// ❌ Avoid
List<Coffee> coffees = new List<Coffee>();
string name = coffee.Name;
```

**Use target-typed `new` for constructor calls:**

```csharp
// ✅ Preferred
List<Coffee> coffees = new();

// ❌ Avoid
List<Coffee> coffees = new List<Coffee>();
```

**Use file-scoped namespaces:**

```csharp
// ✅ Preferred
namespace Coffee2Live.Domain;

public class Coffee { }

// ❌ Avoid
namespace Coffee2Live.Domain
{
    public class Coffee { }
}
```

## Null Handling

**Use null-coalescing and null-conditional operators:**

```csharp
// ✅ Preferred
var name = coffee?.Name ?? string.Empty;

// ❌ Avoid
var name = coffee != null ? coffee.Name : string.Empty;
```

**Use `string.IsNullOrWhiteSpace` rather than manual null/empty checks.**

## LINQ

- Prefer method syntax (`.Where()`, `.Select()`, `.OrderBy()`) over query syntax.
- Keep LINQ chains readable — one operator per line for chains longer than two steps.

```csharp
// ✅ Preferred
var topPicks = coffees
    .OrderByDescending(c => c.Body)
    .ThenBy(c => c.Name)
    .Take(3)
    .ToList();
```

