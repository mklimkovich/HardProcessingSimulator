using System.Collections.Concurrent;

namespace WebApi.Storage.Implementation;

internal class InMemoryStorage : ITaskStorage
{
    private readonly ConcurrentDictionary<string, TaskInfo> _storage = new();

    public Task<bool> TaskExistsAsync(string id)
    {
        return Task.FromResult(_storage.ContainsKey(id));
    }

    public Task<bool> TryCreateTaskAsync(TaskInfo task)
    {
        return Task.FromResult(_storage.TryAdd(task.Id, task));
    }

    public Task<string?> GetTaskDataAsync(string id)
    {
        return Task.FromResult(Find(id)?.Data);
    }

    public Task SaveTaskDataAsync(string id, string data)
    {
        TaskInfo? task = Find(id);
        if (task is not null)
        {
            task.Data = data;
        }

        return Task.CompletedTask;
    }

    public Task<(int LastSentCharacterNumber, int TotalLength)?> GetTaskStatisticsAsync(string id)
    {
        (int LastSentCharacterNumber, int TotalLength)? result = default;

        TaskInfo? task = Find(id);
        if (task is not null)
        {
            result = (task.LastSentCharacterNumber, task.Data.Length);
        }

        return Task.FromResult(result);
    }

    public Task<char?> GetCharacterAsync(string id, int characterNumber)
    {
        char? result = default;

        TaskInfo? task = Find(id);
        if (task is not null)
        {
            result = task.Data[characterNumber];
        }

        return Task.FromResult(result);
    }

    public Task SaveLastSentCharacterAsync(string id, int lastSentCharacterNumber)
    {
        TaskInfo? task = Find(id);
        if (task is not null)
        {
            task.LastSentCharacterNumber = lastSentCharacterNumber;
        }

        return Task.CompletedTask;
    }

    public Task DeleteTaskAsync(string id)
    {
        _storage.TryRemove(id, out _);

        return Task.CompletedTask;
    }

    private TaskInfo? Find(string id)
    {
        _storage.TryGetValue(id, out TaskInfo? task);

        return task;
    }
}