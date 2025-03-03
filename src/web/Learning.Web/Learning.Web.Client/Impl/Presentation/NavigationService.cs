using Learning.Web.Client.Contracts.Presentation;
using Microsoft.AspNetCore.Components;

namespace Learning.Web.Client.Impl.Presentation;

public class NavigationService : INavigationService
{
    private readonly NavigationManager _navigationManager;
    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }
    public void NavigateTo(string url)
    {
        _navigationManager.NavigateTo(url);
    }

    public void NavigateTo(string url, bool forceLoad)
    {
        _navigationManager.NavigateTo(url, forceLoad);
    }

    public void NavigateToLogin(bool suppressRedirect = false)
    {
        _navigationManager.NavigateTo($"/account/login?redirectUrl={(!suppressRedirect ? _navigationManager.ToBaseRelativePath(_navigationManager.Uri) : string.Empty)}", true);
    }

    public string? GetTargetFragmentInRoute()
    {
        var uri = new Uri(_navigationManager.Uri);
        if (!string.IsNullOrEmpty(uri.Fragment))
        {
            var targetId = uri.Fragment.TrimStart('#');
            return !string.IsNullOrEmpty(targetId) ? targetId : null;
        }

        return null;
    }

    public void NavigateToAccountConfirmation(string mobileNumber)
    {
        _navigationManager.NavigateTo($"/account/confirm?username={Uri.EscapeDataString(mobileNumber)}", true);
    }
}
