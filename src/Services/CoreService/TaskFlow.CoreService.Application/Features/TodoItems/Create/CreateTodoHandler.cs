using MediatR;
using TaskFlow.SharedKernel.Interfaces;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Create;

public sealed class CreateTodoHandler(ITodoRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTodoCommand, Result<CreateTodoResponse>>
{
    public async Task<Result<CreateTodoResponse>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var (projectIdResult, authorIdResult) = (
            TodoProjectId.FromGuid(request.ProjectId),
            TodoAuthorId.FromGuid(request.AuthorId)
        );

        var combinedResult = Result.Combine(projectIdResult, authorIdResult);

        if (combinedResult.IsFailure) return Result<CreateTodoResponse>.Failure(combinedResult.Errors);

        var todoResult = Todo.Create(
            projectIdResult.Value,
            authorIdResult.Value,
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate,
            request.EstimatedCompletionTime);

        if (todoResult.IsFailure)
            return Result<CreateTodoResponse>.Failure(todoResult.Errors);

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            await repository.AddAsync(todoResult.Value, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result<CreateTodoResponse>.Success(new CreateTodoResponse(todoResult.Value.Id.Value,
                todoResult.Value.PublicId.Value));
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            return Result<CreateTodoResponse>.Failure(Error.DatabaseUnexpectedError);
        }
    }
}