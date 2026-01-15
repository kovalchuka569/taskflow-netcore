using MediatR;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Create;

public sealed record CreateTodoCommand(
    Guid ProjectId,
    Guid AuthorId,
    string Title,
    string Description,
    TodoPriority? Priority,
    DateTime DueDate,
    DateTime EstimatedCompletionTime) : IRequest<Result<CreateTodoResponse>>;