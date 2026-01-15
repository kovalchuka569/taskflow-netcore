namespace TaskFlow.CoreService.Presentation.Modules.TodoItems;

public sealed record UpdateTodoDescriptionRequest(Guid ProjectId, string NewDescription);