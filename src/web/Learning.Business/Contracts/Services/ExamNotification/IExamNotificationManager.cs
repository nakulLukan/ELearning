using Learning.Business.Dto.ModelExams;
using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification;

namespace Learning.Business.Contracts.Services.ExamNotification;

public interface IExamNotificationManager
{
    public Task<List<ActiveExamNotificationCacheDto>> GetAllActiveExamNotifications(CancellationToken cancellationToken);
    public Task<ExamNotificationDetailCacheDto> GetExamNotificationDetail(int modelExamId, CancellationToken cancellationToken);

}
