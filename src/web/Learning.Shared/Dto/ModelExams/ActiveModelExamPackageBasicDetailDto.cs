namespace Learning.Shared.Dto.ModelExams;

public class ActiveModelExamPackageBasicDetailDto
{
    public required int ExamNotificationId { get; set; }
    public required string ExamNotificationName { get; set; }
    public required string ImageAbsUrl{ get; set; }
    public required bool IsPurchased { get; set; }
}
