using System.Security.Cryptography;
using TaskFlow.SharedKernel.Domain;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems.Constraints;
using TaskFlow.TaskService.Domain.TodoItems.Errors;

namespace TaskFlow.TaskService.Domain.TodoItems;

public sealed class TodoPublicId : ValueObject
{
    public string Value { get; }

    private TodoPublicId(string value)
    {
        Value = value;
    }

    public static TodoPublicId Generate()
    {
        const int length = TodoPublicIdConstraints.Length;
        const string allowedChars = TodoPublicIdConstraints.AllowedChars;

        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);

        var chars = new char[length];
        for (var i = 0; i < length; i++) chars[i] = allowedChars[bytes[i] % allowedChars.Length];

        return new TodoPublicId(new string(chars));
    }

    public static Result<TodoPublicId> FromString(string value)
    {
        switch (value.Length)
        {
            case > TodoPublicIdConstraints.Length:
                return Result<TodoPublicId>.Failure(TodoPublicIdErrors.PublicIdIsTooLong);
            case < TodoPublicIdConstraints.Length:
                return Result<TodoPublicId>.Failure(TodoPublicIdErrors.PublicIdIsTooShort);
        }

        if (value.Any(c => !TodoPublicIdConstraints.AllowedChars.Contains(c)))
            return Result<TodoPublicId>.Failure(TodoPublicIdErrors.PublicIdContainsInvalidCharacters);
        
        return Result<TodoPublicId>.Success(new TodoPublicId(value));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}