namespace TaskFlow.CoreService.Presentation.Modules.TodoItems;

/// <summary>
/// Request to change the task title.
/// </summary>
/// <param name="ProjectId">Todo project identifier.</param>
/// <param name="NewTitle">New title (3-100 symbols).</param>
public sealed record UpdateTodoTitleRequest(Guid ProjectId, string NewTitle);