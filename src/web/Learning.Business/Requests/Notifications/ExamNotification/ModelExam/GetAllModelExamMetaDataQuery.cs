﻿using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam;

public class GetAllModelExamMetaDataQuery : IRequest<GetAllModelExamMetaDataResponseDto[]>
{
    public int ExamNotificationId { get; set; }
}

public class GetAllModelExamMetaDataQueryHandler : IRequestHandler<GetAllModelExamMetaDataQuery, GetAllModelExamMetaDataResponseDto[]>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IApiRequestContext _requestContext;

    public GetAllModelExamMetaDataQueryHandler(
        IAppDbContextFactory appDbContext,
        IApiRequestContext requestContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<GetAllModelExamMetaDataResponseDto[]> Handle(GetAllModelExamMetaDataQuery request, CancellationToken cancellationToken)
    {
        var modelExams = await _appDbContext.ModelExamConfigurations
            .Where(x => x.ExamNotificationId == request.ExamNotificationId)
            .Select(x => new
            {
                ExamNotificationId = request.ExamNotificationId,
                ExamConfigId = x.Id,
                ExamName = x.ExamName,
                ExamDescription = x.Description,
                IsFree = x.IsFree,
                IsLocked = true,
                Price = x.ModelExamPackage!.Price,
                ExamPackageId = x.ModelExamPackageId,
                DiscountedPrice = x.ModelExamPackage.DiscountedPrice,
                ValidUpto = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2))
            })
            .ToListAsync(cancellationToken);

        bool hasPurchased = false;
        if (await _requestContext.IsAuthenticated())
        {
            var userId = await _requestContext.GetUserId();
            hasPurchased = await _appDbContext.ModelExamPurchaseHistory.AnyAsync(x => x.UserId == userId
            && x.ModelExamPackageId == modelExams.First().ExamPackageId, cancellationToken);
        }

        return modelExams.Select(x => new GetAllModelExamMetaDataResponseDto
        {
            DiscountedPrice = x.DiscountedPrice,
            ExamConfigId = x.ExamConfigId,
            ExamName = x.ExamName,
            ExamNotificationId = request.ExamNotificationId,
            IsFree = x.IsFree,
            IsLocked = !x.IsFree && !hasPurchased,
            Price = x.Price,
            ExamDescription = x.ExamDescription,
            ValidUpto = x.ValidUpto
        }).ToArray();
    }
}