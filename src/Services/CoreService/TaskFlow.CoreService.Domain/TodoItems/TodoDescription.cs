using TaskFlow.SharedKernel.Domain;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems.Constraints;
using TaskFlow.TaskService.Domain.TodoItems.Errors;

namespace TaskFlow.TaskService.Domain.TodoItems;

public sealed class TodoDescription : ValueObject
{
    public string Value { get; }

    private TodoDescription(string description)
    {
        Value = description;
    }

    public static Result<TodoDescription> Create(string description)
    {
        if (description.Length > TodoDescriptionConstraints.MaxLength)
            return Result<TodoDescription>.Failure(TodoDescriptionErrors.TodoDescriptionIsTooLong);

        return Result<TodoDescription>.Success(new TodoDescription(description));
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}