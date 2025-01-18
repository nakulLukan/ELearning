namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

internal class ModelExamAssociatedQuestionCacheDto
{
    public int ModelExamId { get; set; }

    /// <summary>
    /// Question ids in ascending order of 'Order'
    /// </summary>
    public int[] QuestionIds { get; set; } = [];
}
