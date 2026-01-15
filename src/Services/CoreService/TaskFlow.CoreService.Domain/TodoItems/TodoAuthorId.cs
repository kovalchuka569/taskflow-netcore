using TaskFlow.SharedKernel.Domain;

namespace TaskFlow.TaskService.Domain.TodoItems;

public sealed class TodoAuthorId : StronglyTypedId<TodoAuthorId>
{
    private TodoAuthorId(Guid value) : base(value)
    {
    }
}