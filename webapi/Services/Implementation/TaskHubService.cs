using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Storage;

namespace WebApi.Services.Implementation;

public class TaskHubService : ITaskHubService
{
    private readonly IHubContext<TaskHub> _hubContext;
    private readonly ITaskStorage _storage;

    public TaskHubService(
        IHubContext<TaskHub> hubContext,
        ITaskStorage storage)
    {
        _hubContext = hubContext;
        _storage = storage;
    }

    public async Task SendOutputAsync(string taskId, char? character, int index, int total, bool isLast)
    {
        if (isLast)
        {
            await _storage.DeleteTaskAsync(taskId);
        }

        await _hubContext.Clients.Client(connectionId: taskId).SendAsync("OnNextCharacterReceived", character, index, total, isLast);
    }
}