namespace WebApi.Interfaces;

public interface IOutputHub
{
    Task SendOutputAsync(string taskId, char? character, bool isLast);
}