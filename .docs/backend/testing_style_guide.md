# Backend Testing Style Guide

## Core Principles

- **Arrange / Act / Assert** – clear three-part structure with a blank line between sections.
- **One concept per test** – multiple `Should()` calls are fine when verifying the same outcome.
- **No magic strings** – derive expected values from the objects under test, not hardcoded literals.
- **Descriptive names** – `Method_Condition_ExpectedBehavior` (e.g., `GetById_ReturnsNotFound_WhenIdDoesNotExist`).
- **Group by class** – one file per class under test (e.g., `CoffeesControllerTests.cs`); use `// --- MethodName ---` comments to separate groups.
- **Fakes over mocks** – use hand-written `sealed` fake classes (e.g., `FakeWebHostEnvironment`), named with a `Fake` prefix, kept at the bottom of the test file.
- **FluentAssertions only** – never use `Assert.AreEqual` or other NUnit-native assertions.

## Writing Tests

```csharp
[Test]
public void GetAll_ReturnsOk_WithCoffees_WhenFileExists()
{
    WriteJson("""[{ "name": "Espresso", ... }]""");

    var result = CreateController().GetAll();

    var ok = result.Result as OkObjectResult;
    ok.Should().NotBeNull();
    (ok!.Value as IEnumerable<Coffee>).Should().HaveCount(1);
}
```

Derive IDs and values from the SUT — never hardcode them:

```csharp
// ✅ Preferred
var expectedId = GetAllCoffees().First().Id;
controller.GetById(expectedId).Result.Should().BeOfType<OkObjectResult>();

// ❌ Avoid
controller.GetById(Guid.Parse("a1b2c3d4-..."));
```

## What to Test

- **Controller actions**: happy path and all error branches (404, 409, 400).
- **Recommendation logic**: any method in `Coffee2Live.Recommendation` that transforms or filters data.
- **Edge cases**: empty file, missing file, null fields, unrecognized enum values.
- Do **not** test `Program.cs` startup or infrastructure wiring.
