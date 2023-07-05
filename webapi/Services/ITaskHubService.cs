namespace WebApi.Services;

public interface ITaskHubService
{
    Task SendOutputAsync(string taskId, char? character, int index, int total, bool isLast);
}