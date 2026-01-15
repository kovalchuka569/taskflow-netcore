using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.SharedKernel.Domain;

public static class StronglyTypedIdErrors
{
    public static Error StronglyTypedIdCannotBeEmpty => new Error("StronglyTypedId.CannotBeEmpty", "StronglyTypedId cannot be empty.");
}