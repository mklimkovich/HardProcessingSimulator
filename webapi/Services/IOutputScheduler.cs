namespace WebApi.Services;

public interface IOutputScheduler
{
    Task ScheduleAsync(string taskId);
}