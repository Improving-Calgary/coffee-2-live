using Coffee2Live.App.Controllers;
using Coffee2Live.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Coffee2Live.Tests;

public class CoffeesControllerTests
{
    private string _tempDir = null!;
    private IWebHostEnvironment _env = null!;
    private string _dataFilePath = null!;

    [SetUp]
    public void SetUp()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var dataDir = Path.Combine(_tempDir, "Data");
        Directory.CreateDirectory(dataDir);
        _dataFilePath = Path.Combine(dataDir, "coffees.json");
        _env = new FakeWebHostEnvironment(_tempDir);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    private void WriteJson(string json) => File.WriteAllText(_dataFilePath, json);

    private CoffeesController CreateController() => new CoffeesController(_env);

    private IEnumerable<Coffee> GetAllCoffees()
    {
        var result = CreateController().GetAll().Result as OkObjectResult;
        return (result!.Value as IEnumerable<Coffee>)!;
    }

    // --- GetAll ---

    [Test]
    public void GetAll_ReturnsOk_WithCoffees_WhenFileExists()
    {
        WriteJson("""
            [{ "name": "Espresso", "origin": "Italy", "tastingNotes": "Bold", "bitterness": 8, "acidity": "Low", "body": 9, "roast": "Dark", "bestFor": "Morning", "price": 3.50 }]
            """);

        var result = CreateController().GetAll();

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var coffees = (ok!.Value as IEnumerable<Coffee>)!.ToList();
        coffees.Should().HaveCount(1);
        coffees.First().Name.Should().Be("Espresso");
    }

    [Test]
    public void GetAll_ReturnsOk_WithEmptyList_WhenFileIsMissing()
    {
        // No file written — data file does not exist
        var result = CreateController().GetAll();

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        (ok!.Value as IEnumerable<Coffee>).Should().BeEmpty();
    }

    // --- GetById ---

    [Test]
    public void GetById_ReturnsOk_WithCorrectCoffee_WhenIdExists()
    {
        WriteJson("""
            [{ "name": "Espresso", "origin": "Italy", "tastingNotes": "Bold", "bitterness": 8, "acidity": "Low", "body": 9, "roast": "Dark", "bestFor": "Morning", "price": 3.50 }]
            """);
        var controller = CreateController();
        var expectedId = ((controller.GetAll().Result as OkObjectResult)!.Value as IEnumerable<Coffee>)!.First().Id;

        var result = controller.GetById(expectedId);

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var coffee = ok!.Value as Coffee;
        coffee!.Id.Should().Be(expectedId);
        coffee.Name.Should().Be("Espresso");
    }

    [Test]
    public void GetById_ReturnsNotFound_WhenIdDoesNotExist()
    {
        WriteJson("[]");

        var result = CreateController().GetById(Guid.NewGuid());

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    // --- Deterministic GUID ---

    [Test]
    public void GetAll_SameName_AlwaysProducesSameId()
    {
        WriteJson("""[{ "name": "Espresso", "bitterness": 5, "body": 5, "price": 3.00 }]""");

        var id1 = GetAllCoffees().First().Id;
        var id2 = GetAllCoffees().First().Id; // fresh controller, fresh load

        id1.Should().Be(id2);
    }

    [Test]
    public void GetAll_GuidIsCaseInsensitive_AndTrimsWhitespace()
    {
        Guid GetIdForName(string name)
        {
            WriteJson($$"""[{ "name": "{{name}}", "bitterness": 5, "body": 5, "price": 3.00 }]""");
            return GetAllCoffees().First().Id;
        }

        var id1 = GetIdForName("Espresso");
        var id2 = GetIdForName("espresso");
        var id3 = GetIdForName(" ESPRESSO ");

        id1.Should().Be(id2).And.Be(id3);
    }

    [Test]
    public void GetAll_NullName_DoesNotThrow_AndProducesConsistentId()
    {
        WriteJson("""[{ "bitterness": 5, "body": 5, "price": 3.00 }]""");

        var act = () => GetAllCoffees().ToList();

        act.Should().NotThrow();
        var id1 = act().First().Id;
        var id2 = act().First().Id;
        id1.Should().Be(id2);
    }

    // --- Enum parsing ---

    [Test]
    public void GetAll_ParsesRoastEnum_CaseInsensitive()
    {
        WriteJson("""[{ "name": "A", "roast": "dark", "bitterness": 5, "body": 5, "price": 3.00 }]""");

        GetAllCoffees().First().Roast.Should().Be(Roast.Dark);
    }

    [Test]
    public void GetAll_ParsesAcidityEnum_CaseInsensitive()
    {
        WriteJson("""[{ "name": "A", "acidity": "HIGH", "bitterness": 5, "body": 5, "price": 3.00 }]""");

        GetAllCoffees().First().Acidity.Should().Be(Acidity.High);
    }

    [Test]
    public void GetAll_UnrecognisedRoast_FallsBackToMedium()
    {
        WriteJson("""[{ "name": "A", "roast": "UNKNOWN_ROAST", "bitterness": 5, "body": 5, "price": 3.00 }]""");

        GetAllCoffees().First().Roast.Should().Be(Roast.Medium);
    }

    [Test]
    public void GetAll_UnrecognisedAcidity_FallsBackToMedium()
    {
        WriteJson("""[{ "name": "A", "acidity": "UNKNOWN", "bitterness": 5, "body": 5, "price": 3.00 }]""");

        GetAllCoffees().First().Acidity.Should().Be(Acidity.Medium);
    }

    // --- Null field coercion ---

    [Test]
    public void GetAll_NullOptionalFields_AreCoercedToEmptyString()
    {
        WriteJson("""[{ "bitterness": 5, "body": 5, "price": 3.00 }]""");

        var coffee = GetAllCoffees().First();

        coffee.Name.Should().Be(string.Empty);
        coffee.Origin.Should().Be(string.Empty);
        coffee.TastingNotes.Should().Be(string.Empty);
        coffee.BestFor.Should().Be(string.Empty);
    }

    // --- Fake IWebHostEnvironment ---

    private sealed class FakeWebHostEnvironment : IWebHostEnvironment
    {
        public FakeWebHostEnvironment(string contentRootPath)
        {
            ContentRootPath = contentRootPath;
            WebRootPath = contentRootPath;
        }

        public string ContentRootPath { get; set; }
        public string WebRootPath { get; set; }
        public string EnvironmentName { get; set; } = "Testing";
        public string ApplicationName { get; set; } = "Coffee2Live.Tests";
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
