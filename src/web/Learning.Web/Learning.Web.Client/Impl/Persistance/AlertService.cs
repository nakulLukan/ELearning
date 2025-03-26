using Learning.Web.Client.Components.Controls.Alerts;
using Learning.Web.Client.Components.Controls.Alerts.ModelExam;
using Learning.Web.Client.Contracts.Presentation;
using Learning.Web.Client.Dto.ExamNotifications.ModelExam;
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
        return res!.Data is bool action && action;
    }

    public async Task<bool> DisplayPublicUserGeneralAlert(string title, string description, string ok)
    {
        var options = new DialogOptions()
        {
            BackdropClick = true,
            NoHeader = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters parameters = new DialogParameters()
        {
            { nameof(PublicUserGeneralDialog.Title), title },
            { nameof(PublicUserGeneralDialog.Description), description },
            { nameof(PublicUserGeneralDialog.ConfirmText), ok },
            { nameof(PublicUserGeneralDialog.HideCancelButton), true },
        };
        var dialog = await _dialogService.ShowAsync<PublicUserGeneralDialog>(string.Empty, parameters, options);
        var res = (await dialog.Result);
        return res!.Data is bool action && action;
    }

    public async Task<bool> DisplayNewQuizAlert()
    {
        var options = new DialogOptions()
        {
            BackdropClick = true,
            NoHeader = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };
        var dialog = await _dialogService.ShowAsync<NewQuizDialog>(string.Empty, options);
        var res = (await dialog.Result);
        return res!.Data is bool action && action;
    }

    public async Task<bool> DisplayStartModelExamAlert(
        string modelExamName,
        string modelExamDescription,
        int totalQuestions,
        int totalTimeInSeconds,
        float positiveMark,
        float negativeMark,
        float totalMarks)
    {
        var options = new DialogOptions()
        {
            BackdropClick = true,
            NoHeader = true,
            Position = DialogPosition.Center,
            CloseButton = true,
            MaxWidth = MaxWidth.Large
        };

        DialogParameters parameters = new DialogParameters()
        {
            { nameof(StartModelExamDialog.ModelExamName), modelExamName },
            { nameof(StartModelExamDialog.ModelExamDescription), modelExamDescription },
            { nameof(StartModelExamDialog.TotalQuestions), totalQuestions },
            { nameof(StartModelExamDialog.TotalTimeSeconds), totalTimeInSeconds},
            { nameof(StartModelExamDialog.TotalMarks), totalMarks},
            { nameof(StartModelExamDialog.PositiveMark), positiveMark},
            { nameof(StartModelExamDialog.NegativeMark), negativeMark},
        };
        var dialog = await _dialogService.ShowAsync<StartModelExamDialog>(string.Empty, parameters, options);
        var res = (await dialog.Result);
        return res!.Data is bool action && action;
    }

    public async Task<bool> DisplayPurchaseExamAlert(ModelExamPurchaseDialogParam parameter)
    {
        var options = new DialogOptions()
        {
            BackdropClick = true,
            NoHeader = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters parameters = new DialogParameters()
        {
            { nameof(PurchaseModelExamDialog.ExamNotificationName), parameter.ExamNotificationName },
            { nameof(PurchaseModelExamDialog.ExamNotificationDescription), parameter.ExamNotificationDescription },
            { nameof(PurchaseModelExamDialog.DiscountedPrice), parameter.DiscountedPrice },
            { nameof(PurchaseModelExamDialog.ValidUpto), parameter.ValidUpto },
        };
        var dialog = await _dialogService.ShowAsync<PurchaseModelExamDialog>(string.Empty, parameters, options);
        var res = (await dialog.Result);
        return res!.Data is bool action && action;
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
