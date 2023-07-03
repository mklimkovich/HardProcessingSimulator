using WebApi.Queues;
using WebApi.Services;
using WebApi.Storage;

namespace WebApi.BackgroundServices;

public sealed class OutputService : IHostedService
{
    private readonly IOutputQueueReader _outputQueue;
    private readonly ITaskStorage _storage;
    private readonly IOutputService _service;

    public OutputService(
        IOutputQueueReader outputQueue,
        ITaskStorage storage,
        IOutputService service)
    {
        _outputQueue = outputQueue;
        _storage = storage;
        _service = service;
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

    private async Task SendOutputHandler(object? sender, TaskEventArgs e)
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

                await _service.SendOutputAsync(taskId, character, isLast);

                await _storage.SaveLastSentCharacterAsync(taskId, next);
            }
        }
    }
}