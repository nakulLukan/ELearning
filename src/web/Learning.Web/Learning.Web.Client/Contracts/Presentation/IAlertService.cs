using Learning.Web.Client.Dto.ExamNotifications.ModelExam;
using Learning.Web.Client.Enums;

namespace Learning.Web.Client.Contracts.Presentation;

public interface IAlertService
{
    Task<bool> DisplayAlert(string title, string description, string ok, AlertTypeEnum alertType = AlertTypeEnum.Info);
    Task<bool> DisplayNewQuizAlert();
    Task<bool> DisplayStartModelExamAlert(string modelExamName, string modelExamDescription);
    Task<bool> DisplayPurchaseExamAlert(ModelExamPurchaseDialogParam parameter);
}

