namespace Learning.Business.Dto.Core.Lesson;

public class LessonListViewItemDto
{
    public int LessionId { get; set; }
    public string LessonName { get; set; }

    /// <summary>
    /// Order within subject
    /// </summary>
    public int Order { get; set; }
    /// <summary>
    /// Duration in seconds
    /// </summary>
    public string Duration { get; set; }

    public bool IsActive { get; set; }

    public bool IsPreviewable { get; set; }
}
