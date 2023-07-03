namespace WebApi.Storage;

public interface ITaskStorage
{
    Task<bool> TaskExistsAsync(string id);

    Task<bool> TryCreateTaskAsync(TaskInfo task);

    Task<string?> GetTaskDataAsync(string id);

    Task SaveTaskDataAsync(string id, string data);

    Task<(int LastSentCharacterNumber, int TotalLength)?> GetTaskStatisticsAsync(string id);

    Task<char?> GetCharacterAsync(string id, int characterNumber);

    Task SaveLastSentCharacterAsync(string id, int lastSentCharacterNumber);
    
    Task DeleteTaskAsync(string id);
}