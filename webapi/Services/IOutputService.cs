namespace WebApi.Services;

public interface IOutputService
{
    Task SendOutputAsync(string taskId, char? character, bool isLast);
}