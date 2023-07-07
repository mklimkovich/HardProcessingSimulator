namespace WebApi.Queues;

public interface IEncodingQueueWriter
{
    ValueTask EnqueueAsync(string taskId);
}