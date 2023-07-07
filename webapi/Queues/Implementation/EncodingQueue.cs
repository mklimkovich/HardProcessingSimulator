namespace WebApi.Queues.Implementation
{
    public class EncodingQueue : IEncodingQueueWriter, IEncodingQueueReader
    {
        private readonly ILogger _logger;

        public EncodingQueue(ILogger logger)
        {
            _logger = logger;
        }

        public event ItemReceivedAsyncHandler<TaskEventArgs>? ItemReceived;

        public ValueTask EnqueueAsync(string taskId)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await FireEventAsync(taskId).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Encoding queue processing error");
                }
            });

            return ValueTask.CompletedTask;
        }

        private async ValueTask FireEventAsync(string taskId)
        {
            if (ItemReceived is not null)
            {
                await ItemReceived(this, new TaskEventArgs { TaskId = taskId });
            }
        }
    }
}
