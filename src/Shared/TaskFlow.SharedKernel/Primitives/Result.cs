namespace TaskFlow.SharedKernel.Primitives;

public class Result
{
    public IReadOnlyList<Error> Errors { get; }
    public bool IsSuccess => !Errors.Any();
    public bool IsFailure => !IsSuccess;
    
    protected Result()
    {
        Errors = new List<Error>().AsReadOnly();
    }

    protected Result(IReadOnlyList<Error> errors)
    {
        if (errors.Count == 0)
            throw new ArgumentException("Collection of errors cannot be null or empty for a failure result.",
                nameof(errors));

        Errors = errors;
    }

    public static Result Success()
    {
        return new Result();
    }
    
    public static Result Failure(Error error)
    {
        return error == Error.None
            ? Success()
            : new Result(new List<Error> { error }.AsReadOnly());
    }

    public static Result Failure(IReadOnlyList<Error> errors)
    {
        return errors.Count == 0
            ? Success()
            : new Result(errors);
    }

    public static Result Combine(params Result[] results)
    {
        var errors = results
            .Where(r => r.IsFailure)
            .SelectMany(r => r.Errors)
            .ToList();

        return errors.Count != 0
            ? Failure(errors)
            : Success();
    }
}

public class Result<T> : Result
{
    private readonly T _value;

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException(
                    "Cannot access Value on a failed result. Check IsSuccess before accessing Value.");

            return _value;
        }
    }

    protected Result(T value)
    {
        _value = value;
    }

    protected Result(IReadOnlyList<Error> errors) : base(errors)
    {
        _value = default!;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public new static Result<T> Failure(Error error)
    {
        return error == Error.None
            ? throw new ArgumentException("Cannot create a failure result with Error.None", nameof(error))
            : new Result<T>(new List<Error> { error }.AsReadOnly());
    }

    public new static Result<T> Failure(IReadOnlyList<Error> errors)
    {
        return errors.Count == 0
            ? throw new ArgumentException("Collection of errors cannot be empty for a failure result.", nameof(errors))
            : new Result<T>(errors);
    }

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<IReadOnlyList<Error>, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value) : onFailure(Errors);
    }

    public static Result<T> Combine(params Result<T>[] results)
    {
        var errors = results
            .Where(r => r.IsFailure)
            .SelectMany(r => r.Errors)
            .ToList();

        if (errors.Count != 0)
            return Failure(errors);

        var firstSuccess = results.First(r => r.IsSuccess);
        return Success(firstSuccess.Value!);
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return Failure(error);
    }
}