using Learning.Core.Contracts.PageModels.Base;
using Learning.Core.Contracts.Services;
using Learning.Core.Enums;
namespace Learning.App.Impl.Services;

public class PageService : IPageService
{
    public bool IsPageBusy => Shell.Current.CurrentPage is IPageModelBase contentPageBase && contentPageBase.IsBusy;

    public void DisplayProgressHUD(string title = "")
    {
#if ANDROID
        AndroidHUD.AndHUD.Shared.Show(Platform.CurrentActivity, title);
#elif IOS
        BigTed.BTProgressHUD.Show(status: title, maskType: BigTed.MaskType.Black);
#endif
    }

    public void DismissProgressHUD()
    {
#if ANDROID
        AndroidHUD.AndHUD.Shared.Dismiss();
#elif IOS
        BigTed.BTProgressHUD.Dismiss();
#endif
    }

    private static int GetHeightRequest(int size) => Math.Min(size, 400);

    public async Task DisplayAlert(string title, string message, string cancel, AlertType alertType = AlertType.Info, string iconImage = "")
    {
    }

    public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel, AlertType alertType = AlertType.Info, string iconImage = "")
    {
        return true;
    }

    public Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);
        return routeParameters != null ? Shell.Current.GoToAsync(shellNavigation, routeParameters) : Shell.Current.GoToAsync(shellNavigation);
    }

    public Task NavigateToAsync(string route, bool animate, IDictionary<string, object>? routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);
        return routeParameters != null ? Shell.Current.GoToAsync(shellNavigation, animate, routeParameters) : Shell.Current.GoToAsync(shellNavigation, animate);
    }

    public Task PopAsync() => Shell.Current.GoToAsync("..");
}