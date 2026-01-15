using MediatR;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Application.Features.TodoItems.UpdateTitle;

public sealed record UpdateTodoTitleCommand(Guid TodoId, Guid ProjectId, string NewTitle) : IRequest<Result>;