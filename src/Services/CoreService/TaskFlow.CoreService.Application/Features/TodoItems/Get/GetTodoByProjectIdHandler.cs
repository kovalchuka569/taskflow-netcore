using MediatR;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Get;

public sealed class GetTodoByProjectIdHandler(ITodoRepository todoRepository)
    : IRequestHandler<GetTodoByProjectIdQuery, Result<IReadOnlyList<GetTodoResponse>>>
{
    public async Task<Result<IReadOnlyList<GetTodoResponse>>> Handle(GetTodoByProjectIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var projectIdResult = TodoProjectId.FromGuid(request.ProjectId);
            if (projectIdResult.IsFailure)
                return Result<IReadOnlyList<GetTodoResponse>>.Failure(projectIdResult.Errors);

            var todos = await todoRepository.GetByProjectIdAsync(projectIdResult.Value, cancellationToken);

            var response = todos
                .Select(GetTodoResponse.FromDomain)
                .ToList();

            return Result<IReadOnlyList<GetTodoResponse>>.Success(response);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<GetTodoResponse>>.Failure(Error.DatabaseUnexpectedError);
        }
    }
}