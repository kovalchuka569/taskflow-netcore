using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Presentation.Modules.TodoItems;

public sealed record UpdateTodoPriorityRequest(Guid ProjectId, TodoPriority NewPriority);