namespace Learning.Business.Constants.Notifications;

public static class ExamNotificationCacheKey
{
    public const string ActiveNotificationsKey = "ActiveNotifications";
    public const string ActiveNotificationsDetailKey = "ActiveNotificationsDetail";
    public static string ModelExamAssociatedQuestionKey(int modelExamId) => $"meaqk-{modelExamId}";
    public static string ExamNotificationDetail(int modelExamId) => $"end-{modelExamId}";
}
