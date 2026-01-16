using MediatR;
using TaskFlow.CoreService.Application.Features.TodoItems.Errors;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Get;

public sealed class GetTodoByIdHandler(ITodoRepository todoRepository) : IRequestHandler<GetTodoByIdQuery, Result<GetTodoResponse>>
{
    public async Task<Result<GetTodoResponse>> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todoIdResult = TodoId.FromGuid(request.TodoId);
        if (todoIdResult.IsFailure) return Result<GetTodoResponse>.Failure(todoIdResult.Errors);

        var todo = await todoRepository.GetByIdAsync(todoIdResult.Value, cancellationToken);
        if (todo is null) return Result<GetTodoResponse>.Failure(TodoErrors.TodoNotFound);

        var response = GetTodoResponse.FromDomain(todo);

        return Result<GetTodoResponse>.Success(response);
    }
}