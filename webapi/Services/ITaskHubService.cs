namespace WebApi.Services;

public interface ITaskHubService
{
    ValueTask SendOutputAsync(string taskId, char? character, int index, int total, bool isLast);
}