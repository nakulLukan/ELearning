using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Learning.Core.Contracts.PageModels.Base;
using Microsoft.Extensions.Logging;

namespace Learning.Core.PageModels;

public partial class PageModelBase : ObservableObject, IPageModelBase
{
    private long _isBusy;
    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;

    protected ILogger<PageModelBase> Logger;

    [ObservableProperty] private bool _isInitialized;

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public PageModelBase(ILogger<PageModelBase> logger)
    {
        InitializeAsyncCommand =
            new AsyncRelayCommand(
                async () =>
                {
                    await IsBusyFor(InitializeAsync);
                    IsInitialized = true;
                },
                AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
        Logger = logger;
    }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task StopAsync()
    {
        return Task.CompletedTask;
    }

    public async Task IsBusyFor(Func<Task> unitOfWork)
    {
        Interlocked.Increment(ref _isBusy);
        OnPropertyChanged(nameof(IsBusy));

        try
        {
            await unitOfWork();
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            OnPropertyChanged(nameof(IsBusy));
        }
    }

    protected void LogException(Exception ex, string? message = null)
    {
        if (string.IsNullOrEmpty(message))
            message = ex.Message;
        Logger.LogError(ex, message);
    }
}