namespace Learning.Web.Client.Models.Home;

public class ExamNotificationDto
{
    public required int ExamNotificationId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImagePath { get; set; }
}
