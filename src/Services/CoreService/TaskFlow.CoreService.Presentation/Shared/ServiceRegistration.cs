using Carter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.CoreService.Presentation.Shared;

public static class ServiceRegistration
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
    }
}