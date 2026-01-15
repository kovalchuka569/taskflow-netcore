using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.TaskService.Domain.TodoItems.Errors;

public static class TodoPublicIdErrors
{
    public static readonly Error PublicIdIsTooLong = new("PublicId.IsTooLong", "Public ID is too long.");
    public static readonly Error PublicIdIsTooShort= new("PublicId.IsTooShort", "Public ID is too short.");
    public static readonly Error PublicIdContainsInvalidCharacters = new("PublicId.ContainsInvalidCharacters", "Public ID contains invalid characters.");
}