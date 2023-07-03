using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using WebApi.Storage;

namespace WebApi.Services.Implementation;

public class OutputService : IOutputService
{
    private readonly IHubContext<TaskHub> _hubContext;
    private readonly ITaskStorage _storage;

    public OutputService(
        IHubContext<TaskHub> hubContext,
        ITaskStorage storage)
    {
        _hubContext = hubContext;
        _storage = storage;
    }

    public async Task SendOutputAsync(string taskId, char? character, bool isLast)
    {
        if (isLast)
        {
            await _storage.DeleteTaskAsync(taskId);
        }

        await _hubContext.Clients.Client(connectionId: taskId).SendAsync("OnNextCharacterReceived", character);
    }
}