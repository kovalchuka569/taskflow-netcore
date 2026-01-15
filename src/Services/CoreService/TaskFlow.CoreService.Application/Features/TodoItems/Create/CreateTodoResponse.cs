namespace TaskFlow.CoreService.Application.Features.TodoItems.Create;

/// <summary>
/// Represents the successful response after todo creation.
/// </summary>
/// <param name="CreatedTodoId">The internal database unique identifier.</param>
/// <param name="CreatedTodoPublicId">The user-friendly public identifier.</param>
public sealed record CreateTodoResponse(Guid CreatedTodoId, string CreatedTodoPublicId);