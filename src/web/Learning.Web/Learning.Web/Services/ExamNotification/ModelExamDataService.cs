using FluentResults;
using Learning.Business.Requests.ModelExams;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Payment;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.ModelExam.Payment;
using Learning.Shared.Dto.ModelExams;
using Learning.Shared.Dto.ModelExams.Payment;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
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

    public async Task<Result<ActiveModelExamPackageBasicDetailDto[]>> GetActiveModelExams()
    {
        try
        {
            var modelExams = await _mediator.Send(new GetActiveModelExamPackagesQuery());
            return modelExams;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
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

    public async Task<Result<ResponseDto<long>>> InitiateModelExamOrder(int examNotificationId)
    {
        try
        {
            var orderId = await _mediator.Send(new ModelExamOrderInitiateCommand()
            {
                ExamNotificationId = examNotificationId
            }).ConfigureAwait(false);
            return orderId;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ModeExamOrderCompleteResponseDto>> CompleteModelExamOrder(long modelExamOrderId, OrderStatusEnum status)
    {
        try
        {
            var orderDetails = await _mediator.Send(new ModelExamOrderCompleteCommand()
            {
                ModelExamOrderId = modelExamOrderId,
                OrderStatus = status
            }).ConfigureAwait(false);
            return orderDetails;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<BeginModelExamResponseDto>> BeginModelExam(int modelExamId)
    {
        try
        {
            var sessionDetails = await _mediator.Send(new BeginModelExamSessionCommand()
            {
                ModelExamId = modelExamId
            }).ConfigureAwait(false);
            return sessionDetails;
        }
        catch (AppApiException ex)
        {
            return Result.Fail(ex.ErrorCode);
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result<GetExamQuestionsListItemResponseDto[]>> GetExamQuestionsList(int modelExamId)
    {
        try
        {
            var questions = await _mediator.Send(new GetExamQuestionsListQuery()
            {
                ModelExamId = modelExamId
            }).ConfigureAwait(false);
            return questions;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }

    }

    public async Task<Result<ModelExamSessionQuestionDetailDto>> GetModelExamQuestionById(long modelExamResultId, int questionId)
    {
        try
        {
            var question = await _mediator.Send(new GetModelExamQuestionByIdQuery()
            {
                ModelExamResultId = modelExamResultId,
                QuestionId = questionId
            }).ConfigureAwait(false);
            return question;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<long>>> SubmitExamQuestionResponse(
        long modelExamResultId,
        int questionId,
        int? selectedAnswerId,
        bool hasSkipped)
    {
        try
        {
            var result = await _mediator.Send(new SubmitExamQuestionResponseCommand()
            {
                HasSkipped = hasSkipped,
                ModelExamResultId = modelExamResultId,
                QuestionId = questionId,
                SelectedAnswerId = selectedAnswerId
            }).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<ModelExamSessionStatusEnum>>> CompleteModelExamSession(long modelExamResultId, ModelExamSessionStatusEnum status)
    {
        try
        {
            var result = await _mediator.Send(new CompleteModelExamSessionCommand()
            {
                ModelExamResultId = modelExamResultId,
                Status = status
            }).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<GetModelExamSummaryResponseDto>> GetModelExamSummary(long modelExamResultId)
    {
        try
        {
            var result = await _mediator.Send(new GetModelExamSummaryQuery()
            {
                ModelExamResultId = modelExamResultId,
            }).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<bool>>> DeleteModelExamSession(long modelExamResultId)
    {
        try
        {
            var result = await _mediator.Send(new DeleteModelExamSessionCommand()
            {
                ModelExamResultId = modelExamResultId,
            }).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ModelExamPurchaseNowDto>> GetModelExamPurchaseDetails(int examNotificationId)
    {
        try
        {
            var result = await _mediator.Send(new ModelExamPurchaseNowQuery()
            {
                ExamNotificationId = examNotificationId,
            }).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ExamNotificationDetailResponseDto>> GetExamNotificationDetailByModelExamId(int modelExamId)
    {
        try
        {
            var result = await _mediator.Send(new GetExamNotificationDetailByModelExamIdQuery()
            {
                ModelExamId = modelExamId,
            }).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ModelExamOrderStepDetailDto>> GetModelExamOrderById(long modelExamOrderId)
    {
        try
        {
            var response = await _mediator.Send(new GetModelExamOrderByIdQuery() { ModelExamOrderId = modelExamOrderId });
            return response;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ModelExamOrderStepDetailDto>> CreateRazorpayOrder(long modelExamOrderId)
    {
        try
        {
            var response = await _mediator.Send(new CreateRazorpayOrderCommand() { ModelExamOrderId = modelExamOrderId });
            return response;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ModelExamPaymentReceipt>> GetPaymentReceipt(long modelExamOrderId)
    {
        try
        {
            var response = await _mediator.Send(new ModelExamReceiptQuery() { ModelExamOrderId = modelExamOrderId });
            return response;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
