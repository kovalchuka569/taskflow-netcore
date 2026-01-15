namespace TaskFlow.TaskService.Domain.TodoItems;

/// <summary>
/// Represents the current lifecycle stage of a task.
/// </summary>
public enum TodoStatus
{
    /// <summary>
    /// Todo has been created but work hasn't started yet.
    /// </summary>
    New = 0,

    /// <summary>
    /// Todo is currently being worked on by a developer.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Development is complete, and the code is being reviewed by peers.
    /// </summary>
    CodeReview = 2,

    /// <summary>
    /// Todo is being verified by QA or automated tests.
    /// </summary>
    Testing = 3,

    /// <summary>
    /// Todo is fully completed and meets the definition of done.
    /// </summary>
    Done = 4
}