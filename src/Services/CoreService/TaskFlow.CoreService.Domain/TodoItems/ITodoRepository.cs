using TaskFlow.SharedKernel.Domain;

namespace TaskFlow.TaskService.Domain.TodoItems;

public interface ITodoRepository : IRepository<Todo, TodoId>
{
    Task<IReadOnlyList<Todo>> GetByProjectIdAsync(TodoProjectId projectId, CancellationToken cancellationToken = default);
    Task<Todo?> GetByTodoIdAndProjectIdAsync(TodoId todoId, TodoProjectId projectId, CancellationToken cancellationToken = default);
}