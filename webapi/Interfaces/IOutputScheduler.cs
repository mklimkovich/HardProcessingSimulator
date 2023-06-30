namespace WebApi.Interfaces;

public interface IOutputScheduler
{
    ValueTask ScheduleAsync(string taskId);
}