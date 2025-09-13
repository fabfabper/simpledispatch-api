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
    }
}