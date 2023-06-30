namespace WebApi.Interfaces;

public interface IQueueReader
{
    public event ItemReceivedAsyncHandler<TaskEventArgs>? ItemReceived;
}