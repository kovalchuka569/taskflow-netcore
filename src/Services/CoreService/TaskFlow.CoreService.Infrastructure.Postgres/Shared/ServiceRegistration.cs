using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Context;
using TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Repositories;
using TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.UnitOfWork;
using TaskFlow.SharedKernel.Interfaces;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Infrastructure.Postgres.Shared;

public static class ServiceRegistration
{
    public static void AddInfrastructurePostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddUnitOfWork();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITodoRepository, TodoRepository>();
    }

    private static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Connection string cannot be empty.");
        

        services.AddDbContext<CoreDbContext>(dbContextOptionBuilder =>
        {
            dbContextOptionBuilder.UseNpgsql(connectionString);
        });
    }
}