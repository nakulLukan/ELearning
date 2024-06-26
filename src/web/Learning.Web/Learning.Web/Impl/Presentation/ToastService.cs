﻿using Learning.Web.Client.Contracts.Presentation;
using MudBlazor;

namespace Learning.Web.Impl.Presentation;

public class ToastService : IToastService
{
    private readonly ISnackbar snackbar;

    public ToastService(ISnackbar snackbar)
    {
        this.snackbar = snackbar;
    }

    public void Success(string content, int duration = 3)
    {
        snackbar.Add(content, Severity.Success);
    }

    public void Error(string content, int duration = 3)
    {
        snackbar.Add(content, Severity.Error);
    }
}