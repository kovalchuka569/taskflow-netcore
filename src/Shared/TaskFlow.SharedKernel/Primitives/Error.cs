namespace TaskFlow.SharedKernel.Primitives;

public sealed record Error(string Code, string? Description = null)
{
    public static readonly Error None = new (string.Empty);

    public static implicit operator Result(Error error) => Result.Failure(error);

    public static readonly Error DatabaseUnexpectedError= new("Database.UnexpectedError", "Database unexpected error.");
}