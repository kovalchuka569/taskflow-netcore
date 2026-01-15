using Microsoft.EntityFrameworkCore;
using TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Configurations;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Context;

public class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskConfiguration).Assembly);
    }
    
    public DbSet<Todo> Tasks { get; set; }
}