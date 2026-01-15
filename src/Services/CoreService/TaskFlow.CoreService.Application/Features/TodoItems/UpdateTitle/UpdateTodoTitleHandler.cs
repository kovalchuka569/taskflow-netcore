using MediatR;
using TaskFlow.CoreService.Application.Features.TodoItems.Errors;
using TaskFlow.SharedKernel.Interfaces;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.UpdateTitle;

public sealed class UpdateTodoTitleHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTodoTitleCommand, Result>
{
    public async Task<Result> Handle(UpdateTodoTitleCommand request, CancellationToken cancellationToken)
    {
        var (todoIdResult, projectIdResult, titleResult) = (
            TodoId.FromGuid(request.TodoId),
            TodoProjectId.FromGuid(request.ProjectId),
            TodoTitle.Create(request.NewTitle)
        );

        var combined = Result.Combine(todoIdResult, projectIdResult, titleResult);
        if (combined.IsFailure) return Result.Failure(combined.Errors);

        var todo = await todoRepository.GetByTodoIdAndProjectIdAsync(todoIdResult.Value, projectIdResult.Value, cancellationToken);
        if (todo is null) return Result.Failure(TodoErrors.TodoNotFound);

        if (Equals(todo.Title, titleResult.Value)) return Result.Success();

        var changeResult = todo.ChangeTitle(titleResult.Value);
        if (changeResult.IsFailure) return Result.Failure(changeResult.Errors);

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            todoRepository.Update(todo);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            return Result.Failure(Error.DatabaseUnexpectedError);
        }

        return Result.Success();
    }
}