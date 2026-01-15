using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Get;

public sealed record GetTodoResponse(
    Guid Id,
    Guid ProjectId,
    Guid AuthorId,
    string PublicId,
    string Title,
    string Description,
    TodoStatus Status,
    TodoPriority Priority,
    DateTime DueDate,
    DateTime EstimatedCompletionTime,
    DateTime CreatedAt,
    DateTime ChangedAt)
{
    public static GetTodoResponse FromDomain(Todo todo)
    {
        return new GetTodoResponse(
            todo.Id.Value,
            todo.ProjectId.Value,
            todo.AuthorId.Value,
            todo.PublicId.Value,
            todo.Title.Value,
            todo.Description.Value,
            todo.Status,
            todo.Priority,
            todo.DueDate,
            todo.EstimatedCompletionTime,
            todo.CreatedAt,
            todo.ChangedAt);
    }
}