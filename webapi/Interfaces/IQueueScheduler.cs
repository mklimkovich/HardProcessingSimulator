namespace WebApi.Interfaces;

public interface IQueueScheduler
{
    ValueTask ScheduleTaskAsync(string taskId, TimeSpan timeout);
}