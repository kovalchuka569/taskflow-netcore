using Microsoft.EntityFrameworkCore;
using TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Context;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Repositories;

public sealed class TodoRepository(CoreDbContext dbContext) : Repository<Todo, TodoId>(dbContext), ITodoRepository
{
    private readonly CoreDbContext _dbContext = dbContext;

    public async Task<IReadOnlyList<Todo>> GetByProjectIdAsync(TodoProjectId projectId, CancellationToken cancellationToken)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Todo?> GetByTodoIdAndProjectIdAsync(TodoId todoId, TodoProjectId projectId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.Id == todoId && t.ProjectId == projectId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}