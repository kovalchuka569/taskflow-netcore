using MediatR;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Get;

public sealed record GetTodosByProjectIdQuery(Guid ProjectId) : IRequest<Result<IReadOnlyList<GetTodoResponse>>>;