﻿namespace Learning.Web.Client.Contracts.Presentation;

public interface INavigationService
{
    void NavigateTo(string url);
    void NavigateTo(string url, bool forceLoad);
    void NavigateToLogin();
    string? GetTargetFragmentInRoute();
}
