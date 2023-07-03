namespace WebApi.Queues;

public interface IEncodingQueueReader
{
    public event ItemReceivedAsyncHandler<TaskEventArgs>? ItemReceived;
}