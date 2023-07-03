namespace WebApi.Queues;

public class TaskEventArgs : EventArgs
{
    public required string TaskId { get; init; }
}