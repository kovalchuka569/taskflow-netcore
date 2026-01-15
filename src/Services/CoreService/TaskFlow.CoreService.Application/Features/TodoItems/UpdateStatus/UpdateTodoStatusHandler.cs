using MediatR;
using TaskFlow.CoreService.Application.Features.TodoItems.Errors;
using TaskFlow.SharedKernel.Interfaces;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.UpdateStatus;

public sealed class UpdateTodoStatusHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateTodoStatusCommand, Result> 
{
    public async Task<Result> Handle(UpdateTodoStatusCommand request, CancellationToken cancellationToken)
    {
        var (todoIdResult, projectIdResult) = (
            TodoId.FromGuid(request.TodoId),
            TodoProjectId.FromGuid(request.ProjectId)
        );
        
        var combined = Result.Combine(todoIdResult, projectIdResult);
        if (combined.IsFailure) return Result.Failure(combined.Errors);
        
        var todo = await todoRepository.GetByTodoIdAndProjectIdAsync(todoIdResult.Value, projectIdResult.Value, cancellationToken);
        if (todo is null) return Result.Failure(TodoErrors.TodoNotFound);

        if (Equals(todo.Status, request.NewStatus)) return Result.Success();
        
        var changeResult = todo.ChangeStatus(request.NewStatus);
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