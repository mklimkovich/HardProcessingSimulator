using WebApi.Interfaces;

namespace WebApi.BackgroundServices;

public sealed class OutputService : IHostedService
{
    private readonly IQueueReader _outputQueue;
    private readonly ITaskStorage _storage;
    private readonly IOutputHub _hub;

    public OutputService(
        IQueueReader outputQueue,
        ITaskStorage storage,
        IOutputHub hub)
    {
        _outputQueue = outputQueue;
        _storage = storage;
        _hub = hub;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _outputQueue.ItemReceived += SendOutputHandler;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _outputQueue.ItemReceived -= SendOutputHandler;

        return Task.CompletedTask;
    }

    private async ValueTask SendOutputHandler(object? sender, TaskEventArgs e)
    {
        string taskId = e.TaskId;

        var statistics = await _storage.GetTaskStatisticsAsync(taskId);
        if (statistics is not null)
        {
            (int lastSent, int total) = statistics.Value;

            int next = lastSent + 1;

            char? character = await _storage.GetCharacterAsync(taskId, next);
            if (character is not null)
            {
                bool isLast = next == total - 1;

                await _hub.SendOutputAsync(taskId, character, isLast);

                await _storage.SaveLastSentCharacterAsync(taskId, next);
            }
        }
    }
}