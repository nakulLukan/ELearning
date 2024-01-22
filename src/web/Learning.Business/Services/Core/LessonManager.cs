namespace Learning.Business.Services.Core;

public class LessonManager
{
    public static bool IsLessonLocked(bool isLessonPreviewable, bool isAuthenticated, bool isAdmin)
    {
        if (isLessonPreviewable || isAdmin)
        {
            return false;
        }

        return true;
    }
}
