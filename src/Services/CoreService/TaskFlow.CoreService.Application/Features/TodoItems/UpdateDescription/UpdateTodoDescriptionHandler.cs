using MediatR;
using TaskFlow.CoreService.Application.Features.TodoItems.Errors;
using TaskFlow.SharedKernel.Interfaces;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems;

namespace TaskFlow.CoreService.Application.Features.TodoItems.UpdateDescription;

public sealed class UpdateTodoDescriptionHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateTodoDescriptionCommand, Result>
{
    public async Task<Result> Handle(UpdateTodoDescriptionCommand request, CancellationToken cancellationToken)
    {
        var (todoIdResult, projectIdResult, descriptionResult) = (
            TodoId.FromGuid(request.TodoId),
            TodoProjectId.FromGuid(request.ProjectId),
            TodoDescription.Create(request.NewDescription)
        );
        
        var combined = Result.Combine(todoIdResult, projectIdResult, descriptionResult);
        if (combined.IsFailure) return Result.Failure(combined.Errors);
        
        var todo = await todoRepository.GetByTodoIdAndProjectIdAsync(todoIdResult.Value, projectIdResult.Value, cancellationToken);
       
        Console.WriteLine($"DATA: {todoIdResult.Value.Value}, {projectIdResult.Value.Value}, {descriptionResult.Value.Value}");
        
        if (todo is null) return Result.Failure(TodoErrors.TodoNotFound);
        
        if (Equals(todo.Description, descriptionResult.Value)) return Result.Success();
        
        var changeResult = todo.ChangeDescription(descriptionResult.Value);
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