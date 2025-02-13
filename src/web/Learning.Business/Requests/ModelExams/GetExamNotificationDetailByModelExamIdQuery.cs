using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Shared.Dto.ModelExams;
using MediatR;

namespace Learning.Business.Requests.ModelExams;

public class GetExamNotificationDetailByModelExamIdQuery : IRequest<ExamNotificationDetailResponseDto>
{
    public required int ModelExamId { get; set; }
}

public class GetExamNotificationDetailByModelExamIdQueryHandler : IRequestHandler<GetExamNotificationDetailByModelExamIdQuery, ExamNotificationDetailResponseDto>
{
    private readonly IExamNotificationManager _notificationManager;
    public GetExamNotificationDetailByModelExamIdQueryHandler(
        IExamNotificationManager notificationManager)
    {
        _notificationManager = notificationManager;
    }

    public async Task<ExamNotificationDetailResponseDto> Handle(GetExamNotificationDetailByModelExamIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _notificationManager.GetExamNotificationDetail(request.ModelExamId, cancellationToken);
        return new ExamNotificationDetailResponseDto()
        {
            ExamNotificationId = data.ExamNotificationId,
            ExamNotificationName = data.ExamNotificationName
        };
    }
}

