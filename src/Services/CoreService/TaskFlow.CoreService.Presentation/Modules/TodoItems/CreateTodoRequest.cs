using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Presentation.Modules.TodoItems;

/// <summary>
/// Represents the request to create a new task.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project this task belongs to.</param>
/// <param name="AuthorId">The unique identifier of the user who creates the task.</param>
/// <param name="Title">The title of the task (min 3, max 100 characters).</param>
/// <param name="Description">Optional detailed description of the task.</param>
/// <param name="Priority">Todo priority level (0 = Low, 1 = Medium, 2 = High).</param>
/// <param name="DueDate">The deadline for the task completion.</param>
/// <param name="EstimatedCompletionTime">The estimated duration to complete the task (e.g., "05:00:00").</param>
public sealed record CreateTodoRequest(
    Guid ProjectId,
    Guid AuthorId,
    string Title,
    string Description,
    TodoPriority? Priority,
    DateTime DueDate,
    DateTime EstimatedCompletionTime);