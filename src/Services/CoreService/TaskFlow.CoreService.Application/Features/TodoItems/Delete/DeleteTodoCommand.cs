using MediatR;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Delete;

public sealed record DeleteTodoCommand(Guid TodoId, Guid ProjectId) : IRequest<Result>;