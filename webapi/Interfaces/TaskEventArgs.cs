namespace WebApi.Interfaces;

public class TaskEventArgs : EventArgs
{
    public required string TaskId { get; init; }
}