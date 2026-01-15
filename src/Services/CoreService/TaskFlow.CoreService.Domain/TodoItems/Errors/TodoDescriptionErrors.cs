using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.TaskService.Domain.TodoItems.Errors;

public static class TodoDescriptionErrors
{
    public static readonly Error TodoDescriptionIsTooLong = new("TodoDescription.IsTooLong", "Todo description is too long.");
}