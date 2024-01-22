namespace Learning.Business.Dto.Core.Lesson;

public class ChapterListViewItemDto
{
    public int ChapterId { get; set; }
    public string ChapterName { get; set; }
    public int SerialNumber { get; set; }

    public List<LessonListViewItemDto> Lessons { get; set; }
}


