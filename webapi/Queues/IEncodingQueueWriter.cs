namespace WebApi.Queues;

public interface IEncodingQueueWriter
{
    Task EnqueueAsync(string taskId);
}