using FluentResults;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam;
using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Web.Client.Contracts.Services.ExamNotification;
using MediatR;

namespace Learning.Web.Services.ExamNotification;

public class ModelExamDataService : IModelExamDataService
{
    private readonly IMediator _mediator;

    public ModelExamDataService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<GetAllModelExamMetaDataResponseDto[]>> GetAllModelExamMetaData(int examNotificationId)
    {
        try
        {
            var examNotifications = await _mediator.Send(new GetAllModelExamMetaDataQuery()
            {
                ExamNotificationId = examNotificationId
            });
            return examNotifications;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<bool>>> CheckUserModelExamSubscriptionQuery(int modelExamId)
    {
        try
        {
            var hasValidSubscription = await _mediator.Send(new CheckUserModelExamSubscriptionQuery()
            {
                ModelExamId = modelExamId
            });
            return hasValidSubscription;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
