namespace Learning.Business.Services.ExamNotification;

public static class ExamNotificationHelper
{
    public static string GetModelExamOrderReferenceId(long modelExamOrderId) => $"#ME{modelExamOrderId}";
}
