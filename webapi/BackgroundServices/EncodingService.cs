using WebApi.Queues;
using WebApi.Services;
using WebApi.Storage;

namespace WebApi.BackgroundServices;

public sealed class EncodingService : IHostedService
{
    private readonly IEncodingQueueReader _encodingQueue;
    private readonly ITaskStorage _storage;
    private readonly IBase64Encoder _encoder;
    private readonly IOutputScheduler _outputScheduler;

    public EncodingService(
        IEncodingQueueReader encodingQueue,
        ITaskStorage storage,
        IBase64Encoder encoder,
        IOutputScheduler outputScheduler)
    {
        _encodingQueue = encodingQueue;
        _storage = storage;
        _encoder = encoder;
        _outputScheduler = outputScheduler;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _encodingQueue.ItemReceived += EncodeTextHandler;

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _encodingQueue.ItemReceived -= EncodeTextHandler;

        return Task.CompletedTask;
    }

    private async ValueTask EncodeTextHandler(object? sender, TaskEventArgs e)
    {
        string taskId = e.TaskId;

        string? rawData = await _storage.GetTaskDataAsync(taskId);

        if (rawData is not null)
        {
            string encodedData = _encoder.Encode(rawData);
            await _storage.SaveTaskDataAsync(taskId, encodedData);

            await _outputScheduler.ScheduleAsync(taskId);
        }
    }
}