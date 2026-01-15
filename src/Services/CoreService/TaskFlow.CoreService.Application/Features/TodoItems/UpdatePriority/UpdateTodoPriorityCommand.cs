using MediatR;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.UpdatePriority;

public sealed record UpdateTodoPriorityCommand(Guid TodoId, Guid ProjectId, TodoPriority NewPriority) : IRequest<Result>;