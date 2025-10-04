using SimpleDispatch.Infrastructure;
using SimpleDispatch.SharedModels.Commands;
using SimpleDispatch.SharedModels.CommandTypes;
using SimpleDispatch.SharedModels.Dtos;

public static class EventsController
{
    public static void MapEventsController(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/events", () =>
        {
            var events = new[]
            {
                new { id = 1, location = "Bahnhofquai 1, 8000 Zürich", position = new[] { 47.3769, 8.5417 }, type = "fire" },
                new { id = 2, location = "Universitätsstrasse 5, 8000 Zürich", position = new[] { 47.3782, 8.5481 }, type = "accident" }
            };
            return events;
        })
        .WithName("GetEvents");

        endpoints.MapPut("/events/{id}", (string id, Event updatedEvent, IConfiguration configuration) =>
        {
            try
            {
                var rabbitHost = configuration["RabbitMq:HostName"] ?? "localhost";
                var queueName = configuration["RabbitMq:EventsQueue"] ?? "events";
                var command = EventCommandConverter.ConvertToCommand(updatedEvent, EventCommandType.UpdateEvent);
                var producer = new MessageProducer<EventCommand>(rabbitHost, queueName, configuration["RabbitMq:Username"], configuration["RabbitMq:Password"]);
                producer.Publish(command);
                Console.WriteLine($"Published update command for event {id}");
                return Results.Accepted();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Results.Problem($"Error updating event: {ex.Message}");
            }
        })
        .WithName("UpdateEvent");

        endpoints.MapPost("/events", (Event newEvent, IConfiguration configuration) =>
        {
            try
            {
                var rabbitHost = configuration["RabbitMq:HostName"] ?? "localhost";
                var queueName = configuration["RabbitMq:EventsQueue"] ?? "events";
                var command = EventCommandConverter.ConvertToCommand(newEvent, EventCommandType.CreateEvent);
                var producer = new MessageProducer<EventCommand>(rabbitHost, queueName, configuration["RabbitMq:Username"], configuration["RabbitMq:Password"]);
                producer.Publish(command);
                Console.WriteLine($"Published create command for event");
                return Results.Accepted();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Results.Problem($"Error creating event: {ex.Message}");
            }
        })
        .WithName("CreateEvent");
    }
}