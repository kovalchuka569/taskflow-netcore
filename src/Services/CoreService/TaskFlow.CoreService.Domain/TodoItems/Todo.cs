using TaskFlow.SharedKernel.Domain;
using TaskFlow.SharedKernel.Primitives;
using TaskFlow.TaskService.Domain.TodoItems.Errors;

namespace TaskFlow.TaskService.Domain.TodoItems;

public sealed class Todo : AggregateRoot<TodoId>
{
    private Todo()
    {
    }

    private Todo(
        TodoId id,
        TodoProjectId projectId,
        TodoAuthorId authorId,
        TodoPublicId publicId,
        TodoTitle title,
        TodoDescription description,
        TodoStatus status,
        TodoPriority priority,
        DateTime dueDate,
        DateTime estimatedCompletionTime,
        DateTime createdAt,
        DateTime changedAt
    )
        : base(id)
    {
        ProjectId = projectId;
        AuthorId = authorId;
        PublicId = publicId;
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
        DueDate = dueDate;
        EstimatedCompletionTime = estimatedCompletionTime;
        CreatedAt = createdAt;
        ChangedAt = changedAt;
    }

    public TodoProjectId ProjectId { get; private set; } = null!;
    public TodoAuthorId AuthorId { get; private set; } = null!;
    public TodoPublicId PublicId { get; private set; } = null!;
    public TodoTitle Title { get; private set; } = null!;
    public TodoDescription Description { get; private set; } = null!;
    public TodoStatus Status { get; private set; }
    public TodoPriority Priority { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime EstimatedCompletionTime { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ChangedAt { get; private set; }

    public static Result<Todo> Create(TodoProjectId projectId, TodoAuthorId authorId, string title, string description,
        TodoPriority? priority,
        DateTime dueDate, DateTime estimatedCompletionTime)
    {
        var titleResult = TodoTitle.Create(title);
        var descriptionResult = TodoDescription.Create(description);

        var combinedResult = Result.Combine(titleResult, descriptionResult);
        if (combinedResult.IsFailure) return Result<Todo>.Failure(combinedResult.Errors);

        var id = TodoId.Create();
        var publicId = TodoPublicId.Generate();
        var status = TodoStatus.New;
        var taskPriority = priority ?? TodoPriority.Low;
        var createdAt = DateTime.UtcNow;
        var changedAt = createdAt;

        return Result<Todo>.Success(
            new Todo(id, projectId, authorId, publicId, titleResult.Value, descriptionResult.Value, status,
                taskPriority, dueDate, estimatedCompletionTime,
                createdAt, changedAt));
    }

    public Result ChangeTitle(TodoTitle title)
    {
        if (Equals(Title, title)) return Result.Success();

        if (Status != TodoStatus.New) return Result.Failure(TodoTitleErrors.TaskTitleCanOnlyBeChangedInNewStatus);

        Title = title;

        Touch();
        return Result.Success();
    }

    public Result ChangeDescription(TodoDescription description)
    {
        if (Equals(Description, description)) return Result.Success();

        Description = description;

        Touch();
        return Result.Success();
    }

    public Result ChangeStatus(TodoStatus newStatus)
    {
        if (Equals(Status, newStatus)) return Result.Success();
        
        if (!Enum.IsDefined(newStatus)) return Result.Failure(TodoStatusErrors.TodoStatusIsInvalid);

        Status = newStatus;

        Touch();
        return Result.Success();
    }

    public Result ChangePriority(TodoPriority newPriority)
    {
        if (Equals(Priority, newPriority)) return Result.Success();
        
        if(!Enum.IsDefined(newPriority)) return Result.Failure(TodoPriorityErrors.TodoPriorityIsInvalid);

        Priority = newPriority;

        Touch();
        return Result.Success();
    }

    private void Touch()
    {
        ChangedAt = DateTime.UtcNow;
    }
}