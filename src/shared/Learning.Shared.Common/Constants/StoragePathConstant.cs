namespace Learning.Shared.Common.Constants;

public class StoragePathConstant
{
    public static string VideoContentBasePath(string fileName, string fileNameSuffix = default)
    {
        if (fileNameSuffix == default)
        {
            fileNameSuffix = Guid.NewGuid().ToString();
        }

        return $"videos/{fileName}-{fileNameSuffix}";
    }

    public static string SubjectThumbnailBasePath(int subjectId)
    {
        return $"subjects/{subjectId}";
    }
}
