using Carter;
using Microsoft.AspNetCore.Routing;

namespace TaskFlow.CoreService.Presentation.Shared;

public static class PresentationBuilder
{
    public static void BuildPresentation(this IEndpointRouteBuilder app)
    {
        app.MapCarter();
    }
}