using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.CoreService.Application.Shared;

public static class ServiceRegistration
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = "-";
            cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly);
        });
    }
}