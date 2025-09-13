public static class UnitsController
{
    public static void MapUnitsController(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/units", () =>
        {
            var units = new[]
            {
                new { id = "A1", statusId = 1, name = "Ambulance 1", position = new[] { 47.3744, 8.5456 }, type = "ambulance" },
                new { id = "F1", statusId = 2, name = "Fire Truck 1", position = new[] { 47.3700, 8.5300 }, type = "firetruck" }
            };
            return units;
        })
        .WithName("GetUnits");
    }
}
