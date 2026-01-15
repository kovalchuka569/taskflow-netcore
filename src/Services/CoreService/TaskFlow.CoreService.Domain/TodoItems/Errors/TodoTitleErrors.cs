using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.TaskService.Domain.TodoItems.Errors;

public static class TodoTitleErrors
{
    public static readonly Error TaskTitleIsTooLong = new("TodoTitle.IsTooLong", "Todo title is too long.");
    public static readonly Error TaskTitleCannotBeEmpty = new("TodoTitle.CannotBeEmpty", "Todo title cannot be empty.");
    public static readonly Error TaskTitleCanOnlyBeChangedInNewStatus = new("TodoTitle.CanOnlyBeChangedInNewStatus", "Todo title can only be changed in the new status.");
}