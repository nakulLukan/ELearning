using Learning.Web.Client.Components.Controls.Alerts;
using Learning.Web.Client.Contracts.Presentation;
using Learning.Web.Client.Enums;
using MudBlazor;

namespace Learning.Web.Client.Impl.Persistance;

public class AlertService : IAlertService
{
    private readonly IDialogService _dialogService;

    public AlertService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public async Task<bool> DisplayAlert(string title, string description, string ok, AlertTypeEnum alertType = AlertTypeEnum.Info)
    {
        var parameters = new DialogParameters<AdminAlertDialog>
        {
            { x=>x.Title ,  title},
            { x=>x.Message ,  description},
            { x=>x.OkText ,  ok},

            { x=>x.OkButtonColor , GetOkButtonColor(alertType)},
        };
        var dialog = await _dialogService.ShowAsync<AdminAlertDialog>("Simple Dialog", parameters);
        var res = (await dialog.Result);
        return res.Data is bool action && action;
    }

    private Color GetOkButtonColor(AlertTypeEnum alertType)
    {
        switch (alertType)
        {
            case AlertTypeEnum.Info: return Color.Primary;
            case AlertTypeEnum.Error: return Color.Error;
            case AlertTypeEnum.Warning: return Color.Warning;
            default: return Color.Primary;
        }
    }
}
