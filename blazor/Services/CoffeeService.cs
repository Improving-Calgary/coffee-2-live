using System.Net.Http.Json;
using blazor.Models;

namespace blazor.Services;

public class CoffeeService(HttpClient http)
{
    public Task<Coffee[]?> ListAsync() =>
        http.GetFromJsonAsync<Coffee[]>("/api/coffees");
}
