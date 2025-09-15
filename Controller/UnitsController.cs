public static class UnitsController
{
    public static void MapUnitsController(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/units", async (HttpClient httpClient, IConfiguration configuration) =>
        {
            try
            {
                var endpoint = configuration["UnitServiceBaseUrl"];
                if (string.IsNullOrEmpty(endpoint))
                {
                    return Results.Problem("UnitServiceBaseUrl configuration is missing");
                }

                endpoint = endpoint.TrimEnd('/') + "/units";
                var response = await httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return Results.Content(jsonContent, "application/json");
                }
                else
                {
                    return Results.Problem($"Failed to fetch units: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return Results.Problem($"Error fetching units: {ex.Message}");
            }
        })
        .WithName("GetUnits");
    }
}
