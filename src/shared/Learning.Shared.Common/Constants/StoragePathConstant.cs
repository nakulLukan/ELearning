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

    public static string PublicExamNotificationBasePath => $"{PUBLIC}/notfications/exam-notification";
}
