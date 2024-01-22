namespace Learning.Business.Dto.VideoPlayback;

public class SubjectPlaylistDto
{
    public List<ContentChapterListItemDto> Chapters { get; set; } = new();
    public bool IsSubscribed { get; set; }
}

public class ContentChapterListItemDto
{
    public int ChapterId { get; set; }
    public string ChapterName { get; set; }

    public List<ContentLessonListItemDto> Lessons { get; set; } = new();
}

public class ContentLessonListItemDto
{
    public int LessonId { get; set; }
    public string LessonName { get; set; }
    public string Duration { get; set; }

    public bool HasCompleted { get; set; }
    public bool IsLocked { get; set; }
}

