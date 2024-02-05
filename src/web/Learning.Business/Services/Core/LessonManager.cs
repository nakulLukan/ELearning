namespace Learning.Business.Services.Core;

public class LessonManager
{
    public static bool IsLessonLocked(bool isLessonPreviewable, bool isSubscribed)
    {
        if (isLessonPreviewable || isSubscribed)
        {
            return false;
        }

        return true;
    }
}
