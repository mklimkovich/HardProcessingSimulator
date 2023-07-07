namespace WebApi.Queues;

public interface IOutputQueueScheduler
{
    ValueTask ScheduleTaskAsync(string taskId, TimeSpan timeout);
}