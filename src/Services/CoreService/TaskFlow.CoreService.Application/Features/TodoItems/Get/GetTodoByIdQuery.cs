using MediatR;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Get;

public sealed record GetTodoByIdQuery(Guid TodoId) : IRequest<Result<GetTodoResponse>>;