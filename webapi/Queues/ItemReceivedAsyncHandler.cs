namespace WebApi.Queues;

public delegate Task ItemReceivedAsyncHandler<in TEventArgs>(object? sender, TEventArgs e)
    where TEventArgs : EventArgs;