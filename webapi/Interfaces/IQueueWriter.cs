namespace WebApi.Interfaces;

public interface IQueueWriter
{
    ValueTask EnqueueAsync(string taskId);
}