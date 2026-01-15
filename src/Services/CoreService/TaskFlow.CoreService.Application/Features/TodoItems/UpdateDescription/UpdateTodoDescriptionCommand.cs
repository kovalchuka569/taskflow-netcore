using MediatR;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Application.Features.TodoItems.UpdateDescription;

public sealed record UpdateTodoDescriptionCommand(Guid TodoId, Guid ProjectId, string NewDescription) : IRequest<Result>;