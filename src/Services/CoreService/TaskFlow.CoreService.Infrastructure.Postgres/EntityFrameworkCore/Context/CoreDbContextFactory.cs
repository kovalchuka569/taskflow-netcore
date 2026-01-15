using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Context;

public class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
{
    public CoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tasks;Username=postgres;Password=1234;");
        return new CoreDbContext(optionsBuilder.Options);
    }
}