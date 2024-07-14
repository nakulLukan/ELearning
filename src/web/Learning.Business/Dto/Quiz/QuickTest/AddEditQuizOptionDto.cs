namespace Learning.Business.Dto.Quiz.QuickTest;

public class AddEditQuizOptionDto
{
    public int OptionOrder { get; set; }
    public string? AnswerText { get; set; }
    public byte[]? AnswerImage { get; set; }
}
