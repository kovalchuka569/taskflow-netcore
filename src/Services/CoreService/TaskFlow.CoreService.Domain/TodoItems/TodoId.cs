using TaskFlow.SharedKernel.Domain;

namespace TaskFlow.TaskService.Domain.TodoItems;

public sealed class TodoId : StronglyTypedId<TodoId>
{
    private TodoId(Guid value) : base(value)
    {
    }
}