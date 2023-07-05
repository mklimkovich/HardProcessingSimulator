using Microsoft.AspNetCore.SignalR;
using WebApi.Queues;
using WebApi.Storage;

namespace WebApi.Hubs;

public class TaskHub : Hub
{
    private readonly ITaskStorage _storage;
    private readonly IEncodingQueueWriter _encodingQueue;

    public TaskHub(
        ITaskStorage storage,
        IEncodingQueueWriter encodingQueue)
    {
        _storage = storage;
        _encodingQueue = encodingQueue;
    }

    public async Task StartEncoding(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            await Clients.Caller.SendAsync("OnError", "Text cannot be empty.");
            return;
        }

        string taskId = Context.ConnectionId;

        bool success = await _storage.TryCreateTaskAsync(new TaskInfo(taskId, text));
        if (!success)
        {
            await Clients.Caller.SendAsync("OnError", "Cannot run more than one task.");
            return;
        }

        await _encodingQueue.EnqueueAsync(taskId);
    }

    public Task StopEncoding()
    {
        return OnDisconnectedAsync(null);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _storage.DeleteTaskAsync(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}