using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace SimpleDispatch.Controllers
{
    public static class ConfigurationsController
    {
        public static void MapConfigurationsController(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/configurations/unitstatus", async () =>
            {
                try
                {
                    var filePath = Path.Combine("Configuration", "Unit", "UnitStatus.json");
                    if (!File.Exists(filePath))
                    {
                        return Results.NotFound("Unit status configuration not found");
                    }

                    var jsonContent = await File.ReadAllTextAsync(filePath);
                    return Results.Content(jsonContent, "application/json");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error reading unit status configuration: {ex.Message}");
                }
            })
            .WithName("GetUnitStatus");

            endpoints.MapGet("/configurations/unittypes", async () =>
            {
                try
                {
                    var filePath = Path.Combine("Configuration", "Unit", "UnitTypes.json");
                    if (!File.Exists(filePath))
                    {
                        return Results.NotFound("Unit types configuration not found");
                    }

                    var jsonContent = await File.ReadAllTextAsync(filePath);
                    return Results.Content(jsonContent, "application/json");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error reading unit types configuration: {ex.Message}");
                }
            })
            .WithName("GetUnitTypes");

            endpoints.MapGet("/configurations/eventtypes", async () =>
            {
                try
                {
                    var filePath = Path.Combine("Configuration", "Event", "EventTypes.json");
                    if (!File.Exists(filePath))
                    {
                        return Results.NotFound("Event types configuration not found");
                    }

                    var jsonContent = await File.ReadAllTextAsync(filePath);
                    return Results.Content(jsonContent, "application/json");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error reading event types configuration: {ex.Message}");
                }
            })
            .WithName("GetEventTypes");
        }
    }
}