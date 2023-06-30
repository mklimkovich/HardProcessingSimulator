namespace WebApi.Interfaces;

public interface ITaskStorage
{
    ValueTask<bool> TaskExistsAsync(string id);

    ValueTask<bool> TryCreateTaskAsync(TaskInfo task);

    ValueTask<string?> GetTaskDataAsync(string id);

    ValueTask SaveTaskDataAsync(string id, string data);

    ValueTask<(int LastSentCharacterNumber, int TotalLength)?> GetTaskStatisticsAsync(string id);

    ValueTask<char?> GetCharacterAsync(string id, int characterNumber);

    ValueTask SaveLastSentCharacterAsync(string id, int lastSentCharacterNumber);
    
    ValueTask DeleteTaskAsync(string id);
}