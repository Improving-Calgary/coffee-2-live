using Microsoft.AspNetCore.Mvc;
using Coffee2Live.Domain;
using System.IO;
using System.Linq;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;

namespace Coffee2Live.App.Controllers
{
    /// <summary>
    /// Controller for managing coffees
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CoffeesController : ControllerBase
    {
        private readonly Lazy<List<Coffee>> _coffees;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoffeesController"/> class.
        /// </summary> 
        /// <param name="env">The web host environment</param>   
        public CoffeesController(IWebHostEnvironment env)
        {
            var dataPath = Path.Combine(env.ContentRootPath, "Data", "coffees.json");
            _coffees = new Lazy<List<Coffee>>(() => LoadCoffees(dataPath));
        }


        /// <summary>
        /// Get all coffees
        /// </summary>
        /// <response code="200">Returns the list of coffees</response>
        [HttpGet]
        public ActionResult<IEnumerable<Coffee>> GetAll()
        {
            return Ok(_coffees.Value);
        }

        /// <summary>
        /// Get a coffee by its ID
        /// </summary>
        /// <param name="id">The ID of the coffee</param>
        /// <response code="200">Returns the coffee with the specified ID</response>
        /// <response code="404">If no coffee with the specified ID is found</response>
        [HttpGet("{id}")]
        public ActionResult<Coffee> GetById(Guid id)
        {
            var coffee = _coffees.Value.FirstOrDefault(c => c.Id == id);
            if (coffee == null) return NotFound();
            return Ok(coffee);
        }

        private static List<Coffee> LoadCoffees(string path)
        {
            if (!System.IO.File.Exists(path)) return new List<Coffee>();
            var json = System.IO.File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<List<CoffeeDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<CoffeeDto>();

            var list = new List<Coffee>();
            foreach (var i in items)
            {
                var coffee = new Coffee
                {
                    Id = CreateDeterministicGuid(i.Name),
                    Name = i.Name ?? string.Empty,
                    Origin = i.Origin ?? string.Empty,
                    TastingNotes = i.TastingNotes ?? string.Empty,
                    Bitterness = i.Bitterness,
                    Body = i.Body,
                    BestFor = i.BestFor ?? string.Empty,
                    Acidity = TryParseEnum<Acidity>(i.Acidity, Acidity.Medium),
                    Roast = TryParseEnum<Roast>(i.Roast, Roast.Medium),
                    Price = i.Price
                };
                list.Add(coffee);
            }
            return list;
        }

        private static TEnum TryParseEnum<TEnum>(string? value, TEnum fallback) where TEnum : struct
        {
            if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse<TEnum>(value, true, out var parsed))
                return parsed;
            return fallback;
        }

        private static Guid CreateDeterministicGuid(string? name)
        {
            var normalized = (name ?? string.Empty).Trim().ToLowerInvariant();
            var bytes = System.Text.Encoding.UTF8.GetBytes(normalized);
            var hash = MD5.HashData(bytes); // 16 bytes
            return new Guid(hash);
        }

        private class CoffeeDto
        {
            public string? Name { get; set; }
            public string? Origin { get; set; }
            public string? TastingNotes { get; set; }
            public int Bitterness { get; set; }
            public string? Acidity { get; set; }
            public int Body { get; set; }
            public string? Roast { get; set; }
            public string? BestFor { get; set; }
            public decimal Price { get; set; }
        }
    }
}
