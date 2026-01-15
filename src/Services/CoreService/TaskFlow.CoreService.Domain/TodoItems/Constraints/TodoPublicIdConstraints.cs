namespace TaskFlow.TaskService.Domain.TodoItems.Constraints;

public static class TodoPublicIdConstraints
{
    public const int Length = 8;
    public const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
}