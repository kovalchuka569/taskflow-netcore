using MediatR;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Get;

public sealed record GetTodoByProjectIdQuery(Guid ProjectId) : IRequest<Result<IReadOnlyList<GetTodoResponse>>>;