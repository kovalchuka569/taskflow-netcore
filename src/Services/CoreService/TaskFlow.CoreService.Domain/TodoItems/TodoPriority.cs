namespace TaskFlow.TaskService.Domain.TodoItems;

/// <summary>
/// Defines the urgency and importance of a task within the project.
/// </summary>
public enum TodoPriority
{
    /// <summary>
    /// Minor tasks that have little impact on project progress. Can be deferred.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Routine tasks that should be completed as part of the normal workflow.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// Important tasks that significantly affect project goals or milestones.
    /// </summary>
    High = 3,

    /// <summary>
    /// Urgent issues (e.g., blockers or production bugs) that require immediate action.
    /// </summary>
    Critical = 4
}