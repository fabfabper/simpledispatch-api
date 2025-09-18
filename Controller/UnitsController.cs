using SimpleDispatch.Infrastructure;
using SimpleDispatch.SharedModels.Commands;
using SimpleDispatch.SharedModels.CommandTypes;
using SimpleDispatch.SharedModels.Dtos;

public static class UnitsController
{
    public static void MapUnitsController(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/units", async (HttpClient httpClient, IConfiguration configuration) =>
        {
            try
            {
                var unitsEndpoint = configuration["UnitServiceBaseUrl"];
                if (string.IsNullOrEmpty(unitsEndpoint))
                {
                    return Results.Problem("UnitServiceBaseUrl configuration is missing");
                }

                var response = await httpClient.GetAsync(unitsEndpoint);
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

        endpoints.MapPut("/units/{id}", (string id, Unit updatedUnit, IConfiguration configuration) =>
        {
            try
            {
                var rabbitHost = configuration["RabbitMq:HostName"] ?? "localhost";
                var queueName = configuration["RabbitMq:UnitsQueue"] ?? "units";
                var command = UnitCommandConverter.ConvertToCommand(updatedUnit, UnitCommandType.UpdateUnit);
                var producer = new MessageProducer<UnitCommand>(rabbitHost, queueName);
                producer.Publish(command);
                return Results.Accepted();
            }
            catch (Exception ex)
            {
                return Results.Problem($"Error updating unit: {ex.Message}");
            }
        })
        .WithName("UpdateUnit");

        endpoints.MapPost("/units", (Unit newUnit, IConfiguration configuration) =>
        {
            try
            {
                var rabbitHost = configuration["RabbitMq:HostName"] ?? "localhost";
                var queueName = configuration["RabbitMq:UnitsQueue"] ?? "units";
                var command = UnitCommandConverter.ConvertToCommand(newUnit, UnitCommandType.CreateUnit);
                var producer = new MessageProducer<UnitCommand>(rabbitHost, queueName);
                producer.Publish(command);
                return Results.Accepted();
            }
            catch (Exception ex)
            {
                return Results.Problem($"Error creating unit: {ex.Message}");
            }
        })
        .WithName("CreateUnit");
    }
}
