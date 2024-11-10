using FluentResults;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;

namespace Learning.Web.Client.Contracts.Services.ExamNotification;

public interface IModelExamDataService
{
    public Task<Result<GetAllModelExamMetaDataResponseDto[]>> GetAllModelExamMetaData(int examNotificationId);
}
