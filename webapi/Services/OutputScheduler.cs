﻿using WebApi.Interfaces;

namespace WebApi.Services;

public class OutputScheduler : IOutputScheduler
{
    private const int MinTimeoutSeconds = 1;
    private const int MaxTimeoutSeconds = 5;
    private readonly IQueueScheduler _outputQueue;
    private readonly ITaskStorage _storage;
    private readonly Random _random;

    public OutputScheduler(IQueueScheduler outputQueue, ITaskStorage storage)
    {
        _outputQueue = outputQueue;
        _storage = storage;
        _random = new Random();
    }

    public async ValueTask ScheduleAsync(string taskId)
    {
        if (await _storage.TaskExistsAsync(taskId))
        {
            int timeoutSeconds = _random.Next(MinTimeoutSeconds, MaxTimeoutSeconds);
            TimeSpan timeout = TimeSpan.FromSeconds(timeoutSeconds);

            await _outputQueue.ScheduleTaskAsync(taskId, timeout);
        }
    }
}