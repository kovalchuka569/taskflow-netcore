using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Presentation.Modules.TodoItems;

public sealed record UpdateTodoStatusRequest(Guid ProjectId, TodoStatus NewStatus);