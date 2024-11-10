using Learning.Web.Client.Enums;

namespace Learning.Web.Client.Contracts.Presentation;

public interface IAlertService
{
    Task<bool> DisplayAlert(string title, string description, string ok, AlertTypeEnum alertType = AlertTypeEnum.Info);
    Task<bool> DisplayNewQuizAlert();
}

