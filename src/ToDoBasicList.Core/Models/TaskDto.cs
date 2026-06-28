namespace ToDoBasicList.Core.Models
{
    /// <summary>
    /// Serializable representation of a user task used for persistence.
    /// </summary>
    /// <param name="Description"> The task text </param>
    /// <param name="IsCompleted"> Whether the task is marked as done </param>
    public sealed record TaskDto(string Description, bool IsCompleted);
}
