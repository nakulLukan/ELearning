namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;

public class AddEditModelExamOptionDto
{
    public int OptionId { get; set; }
    public int OptionOrder { get; set; }
    public string? AnswerText { get; set; }
    public byte[]? AnswerImage { get; set; }
    public bool IsCorrectOption { get; set; }
}
