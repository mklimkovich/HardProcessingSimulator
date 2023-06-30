using System.Threading.Tasks.Sources;
using Microsoft.AspNetCore.SignalR;
using WebApi.Interfaces;

namespace WebApi.Hubs;

public class TaskHub : Hub, IOutputHub
{
    private readonly ITaskStorage _storage;
    private readonly IQueueWriter _encodingQueue;

    public TaskHub(
        ITaskStorage storage, 
        IQueueWriter encodingQueue) =>
        (_storage, _encodingQueue) = (storage, encodingQueue);

    public async Task RequestEncoding(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            await Clients.Caller.SendAsync("Error", "Text cannot be empty.");
            return;
        }

        string taskId = Context.ConnectionId;

        bool success = await _storage.TryCreateTaskAsync(new TaskInfo(taskId, text));
        if (!success)
        {
            await Clients.Caller.SendAsync("Error", "Cannot run more than one task.");
            return;
        }

        await _encodingQueue.EnqueueAsync(taskId);
    }

    public async Task SendOutputAsync(string taskId, char? character, bool isLast)
    {
        if (isLast)
        {
            await _storage.DeleteTaskAsync(Context.ConnectionId);
        }

        await Clients.Clients(connection1: taskId).SendAsync("OnNextCharacterReceived", character);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _storage.DeleteTaskAsync(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}