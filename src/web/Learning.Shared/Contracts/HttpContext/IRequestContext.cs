﻿namespace Learning.Shared.Contracts.HttpContext;

public interface IRequestContext
{
    public Task<bool> IsAuthenticated();
    public Task<(bool IsAuthenticated, bool IsAdmin)> IsLoggedIn();
    public Task<bool> IsAdmin();
    public Task<string> GetUserId();
    public Task<string?> GetEmail();
    public Task<string> GetPhoneNumber();
    public Task<string> GetName();
    public Task<string> GetUserRole();
    public Task<string> GetAccessToken();
}
