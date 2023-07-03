namespace WebApi.Queues;

public interface IOutputQueueReader
{
    public event ItemReceivedAsyncHandler<TaskEventArgs>? ItemReceived;
}