using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;

namespace SimpleDispatch.Controllers
{
    public static class TranslationsController
    {
        public static void MapTranslationsController(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/translations/{language}", async (string language) =>
            {
                try
                {
                    var filePath = Path.Combine("Translation", language, $"translation_{language}.json");
                    if (!File.Exists(filePath))
                    {
                        return Results.NotFound($"Translation for language '{language}' not found");
                    }

                    var jsonContent = await File.ReadAllTextAsync(filePath);
                    return Results.Content(jsonContent, "application/json");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error reading translation for language '{language}': {ex.Message}");
                }
            })
            .WithName("GetTranslations");
        }
    }
}