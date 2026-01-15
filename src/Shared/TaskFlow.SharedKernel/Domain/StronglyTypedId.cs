using System.Reflection;
using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.SharedKernel.Domain;

public abstract class StronglyTypedId<TEntityId>(Guid value) : ValueObject
    where TEntityId : StronglyTypedId<TEntityId>
{
    public Guid Value { get; } = value;

    private static TEntityId CreateInstance(Guid value)
    {
        var type = typeof(TEntityId);

        var constructor = type.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [typeof(Guid)],
            null);

        if (constructor == null)
            throw new InvalidOperationException(
                $"Derived StronglyTypedId '{type.Name}' must have a private constructor accepting a single Guid argument.");

        return (TEntityId)constructor.Invoke([value]);
    }

    public static TEntityId Create()
    {
        return CreateInstance(Guid.NewGuid());
    }

    public static Result<TEntityId> FromGuid(Guid value)
    {
        if (value == Guid.Empty)
            return Result<TEntityId>.Failure(StronglyTypedIdErrors.StronglyTypedIdCannotBeEmpty);

        return Result<TEntityId>.Success(CreateInstance(value));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}