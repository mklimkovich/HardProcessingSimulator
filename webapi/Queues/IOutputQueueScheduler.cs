namespace WebApi.Queues;

public interface IOutputQueueScheduler
{
    Task ScheduleTaskAsync(string taskId, TimeSpan timeout);
}