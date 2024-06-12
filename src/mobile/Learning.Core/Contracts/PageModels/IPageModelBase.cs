using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Learning.Core.Contracts.PageModels.Base;

public interface IPageModelBase : INotifyPropertyChanged
{
    public void ApplyQueryAttributes(IDictionary<string, object> query);
    public IAsyncRelayCommand InitializeAsyncCommand { get; }
    public bool IsBusy { get; }
    public bool IsInitialized { get; }
    Task InitializeAsync();
    Task StopAsync();
}