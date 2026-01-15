using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.TaskService.Domain.TodoItems.Errors;

public static class TodoStatusErrors
{
    public static readonly Error TodoStatusIsInvalid = new("TodoStatus.IsInvalid", "The provided status is not valid.");
}