using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.TaskService.Domain.TodoItems.Errors;

public static class TodoPriorityErrors
{
    public static readonly Error TodoPriorityIsInvalid = new("TodoPriority.IsInvalid", "The provided priority is not valid.");
}