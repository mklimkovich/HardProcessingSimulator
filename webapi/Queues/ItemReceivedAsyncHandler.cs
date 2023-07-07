namespace WebApi.Queues;

public delegate ValueTask ItemReceivedAsyncHandler<in TEventArgs>(object? sender, TEventArgs e)
    where TEventArgs : EventArgs;