using MediatR;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.UpdateStatus;

public sealed record UpdateTodoStatusCommand(Guid TodoId, Guid ProjectId, TodoStatus NewStatus) : IRequest<Result>;