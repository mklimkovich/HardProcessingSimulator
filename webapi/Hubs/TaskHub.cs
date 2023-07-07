using Microsoft.AspNetCore.SignalR;
using WebApi.Queues;
using WebApi.Storage;

namespace WebApi.Hubs;

public class TaskHub : Hub
{
    private const string ErrorClientMethod = "OnError";
    private const string EmptyTextValidationError = "Text cannot be empty.";
    private const string UniquenessValidationError = "Cannot run more than one task.";

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
            await Clients.Caller.SendAsync(ErrorClientMethod, EmptyTextValidationError);
            return;
        }

        string taskId = Context.ConnectionId;

        bool success = await _storage.TryCreateTaskAsync(new TaskInfo(taskId, text));
        if (!success)
        {
            await Clients.Caller.SendAsync(ErrorClientMethod, UniquenessValidationError);
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