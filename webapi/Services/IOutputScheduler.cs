namespace WebApi.Services;

public interface IOutputScheduler
{
    ValueTask ScheduleAsync(string taskId);
}