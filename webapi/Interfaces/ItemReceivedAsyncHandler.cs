namespace WebApi.Interfaces;

public delegate ValueTask ItemReceivedAsyncHandler<in TEventArgs>(object? sender, TEventArgs e)
    where TEventArgs : EventArgs;