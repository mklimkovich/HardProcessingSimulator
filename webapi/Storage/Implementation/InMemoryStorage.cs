using System.Collections.Concurrent;

namespace WebApi.Storage.Implementation;

internal class InMemoryStorage : ITaskStorage
{
    private readonly ConcurrentDictionary<string, TaskInfo> _storage = new();

    public ValueTask<bool> TaskExistsAsync(string id)
    {
        return ValueTask.FromResult(_storage.ContainsKey(id));
    }

    public ValueTask<bool> TryCreateTaskAsync(TaskInfo task)
    {
        return ValueTask.FromResult(_storage.TryAdd(task.Id, task));
    }

    public ValueTask<string?> GetTaskDataAsync(string id)
    {
        return ValueTask.FromResult(Find(id)?.Data);
    }

    public ValueTask SaveTaskDataAsync(string id, string data)
    {
        TaskInfo? task = Find(id);
        if (task is not null)
        {
            task.Data = data;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask<(int LastSentCharacterNumber, int TotalLength)?> GetTaskStatisticsAsync(string id)
    {
        (int LastSentCharacterNumber, int TotalLength)? result = default;

        TaskInfo? task = Find(id);
        if (task is not null)
        {
            result = (task.LastSentCharacterNumber, task.Data.Length);
        }

        return ValueTask.FromResult(result);
    }

    public ValueTask<char?> GetCharacterAsync(string id, int characterNumber)
    {
        char? result = default;

        TaskInfo? task = Find(id);
        if (task is not null)
        {
            result = task.Data[characterNumber];
        }

        return ValueTask.FromResult(result);
    }

    public ValueTask SaveLastSentCharacterAsync(string id, int lastSentCharacterNumber)
    {
        TaskInfo? task = Find(id);
        if (task is not null)
        {
            task.LastSentCharacterNumber = lastSentCharacterNumber;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask DeleteTaskAsync(string id)
    {
        _storage.TryRemove(id, out _);

        return ValueTask.CompletedTask;
    }

    private TaskInfo? Find(string id)
    {
        _storage.TryGetValue(id, out TaskInfo? task);

        return task;
    }
}