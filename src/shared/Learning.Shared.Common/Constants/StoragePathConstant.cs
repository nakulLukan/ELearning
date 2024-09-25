namespace Learning.Shared.Common.Constants;

public class StoragePathConstant
{
    public const string PRIVATE = "private";
    public const string PUBLIC = "public";

    public static string VideoContentBasePath(string fileName, string fileNameSuffix = default)
    {
        if (fileNameSuffix == default)
        {
            fileNameSuffix = Guid.NewGuid().ToString();
        }

        return $"{PRIVATE}/videos/{fileName}-{fileNameSuffix}";
    }

    public static string SubjectThumbnailBasePath(int subjectId)
    {
        return $"{PRIVATE}/subjects/{subjectId}";
    }

    public static string PublicExamNotificationBasePath(int examNotificationId) => $"{PUBLIC}/notfications/exam-notification/{examNotificationId}";
    public static string QuizOptionImageBasePath(int quizConfigId) => $"{PUBLIC}/quiz/quick_test/configuration_{quizConfigId}";

    public static string ExamNotificationModelExamBasePath(int examNotificationId, int modelExamId) => $"{PRIVATE}/notification/exam-notification/{examNotificationId}/model-exams/{modelExamId}";
    public static string ModelExamSolutionVideoBasPath(int examNotificationId, int modelExamId) => $"{ExamNotificationModelExamBasePath(examNotificationId, modelExamId)}/solution";
}
