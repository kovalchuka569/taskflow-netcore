using TaskFlow.SharedKernel.Primitives;

namespace TaskFlow.CoreService.Application.Features.TodoItems.Errors;

public static class TodoErrors
{
    public static readonly Error TodoNotFound = new("Todo.NotFound", "Todo not found");
}