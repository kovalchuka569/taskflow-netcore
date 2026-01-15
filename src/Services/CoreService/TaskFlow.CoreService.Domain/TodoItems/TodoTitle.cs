using TaskFlow.SharedKernel.Domain;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems.Constraints;
using TaskFlow.TaskService.Domain.TodoItems.Errors;

namespace TaskFlow.TaskService.Domain.TodoItems;

public sealed class TodoTitle : ValueObject
{
    public string Value { get; }

    private TodoTitle(string title)
    {
        Value = title;
    }

    public static Result<TodoTitle> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) 
            return Result<TodoTitle>.Failure(TodoTitleErrors.TaskTitleCannotBeEmpty);

        if (title.Length > TodoTitleConstraints.MaxLength)
            return Result<TodoTitle>.Failure(TodoTitleErrors.TaskTitleIsTooLong);

        return Result<TodoTitle>.Success(new TodoTitle(title));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}