using Hangfire;

namespace WebApi.Queues.Implementation;

public class OutputQueue : IOutputQueueScheduler, IOutputQueueReader
{
    private readonly ILogger _logger;

    public OutputQueue(ILogger logger)
    {
        _logger = logger;
    }

    public event ItemReceivedAsyncHandler<TaskEventArgs>? ItemReceived;

    public Task ScheduleTaskAsync(string taskId, TimeSpan timeout)
    {
        BackgroundJob.Schedule(() => EnqueueAsync(taskId), timeout);

        return Task.CompletedTask;
    }

    public async Task EnqueueAsync(string taskId)
    {
        try
        {
            await FireEventAsync(taskId).ConfigureAwait(false);
        }
        catch (Exception e)
        {
           _logger.LogError(e, "Encoding queue processing error");
        }
    }

    private async Task FireEventAsync(string taskId)
    {
        if (ItemReceived is not null)
        {
            await ItemReceived(this, new TaskEventArgs { TaskId = taskId });
        }
    }
}
