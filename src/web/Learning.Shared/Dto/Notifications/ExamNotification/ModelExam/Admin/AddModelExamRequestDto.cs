namespace Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.Admin;

public class AddModelExamRequestDto
{
    public required int Id { get; set; }
    public required int ExamNotificationId { get; set; }
    public required string ExamName { get; set; }
    public required string ExamDescription { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsFree { get; set; }
    public required float Price { get; set; }
    public required float DiscountedPrice { get; set; }
    public required int TotalTimeLimit { get; set; }
    public required string? SolutionFileName { get; set; }
}
