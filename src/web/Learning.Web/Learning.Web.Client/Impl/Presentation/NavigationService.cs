﻿using Learning.Web.Client.Contracts.Presentation;
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

    public void NavigateToLogin()
    {
        _navigationManager.NavigateTo($"/login?redirectUrl={_navigationManager.ToBaseRelativePath(_navigationManager.Uri)}");
    }
}