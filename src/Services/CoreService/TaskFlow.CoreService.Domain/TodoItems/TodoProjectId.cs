using TaskFlow.SharedKernel.Domain;

namespace TaskFlow.TaskService.Domain.TodoItems;

public sealed class TodoProjectId : StronglyTypedId<TodoProjectId>
{
    private TodoProjectId(Guid value) : base(value)
    {
    }
}