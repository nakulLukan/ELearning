using Learning.Core.Enums;

namespace Learning.Core.Contracts.Services;

public interface IPageService
{
    bool IsPageBusy { get; }
    
    void DisplayProgressHUD(string title = "");
    void DismissProgressHUD();

    Task DisplayAlert(string title, string message, string cancel, AlertType alertType = AlertType.Info, string iconImage = "");
    Task<bool> DisplayAlert(string title, string message, string accept, string cancel, AlertType alertType = AlertType.Info, string iconImage = "");

    Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null);
    Task NavigateToAsync(string route, bool animate, IDictionary<string, object>? routeParameters = null);
    Task PopAsync();
}