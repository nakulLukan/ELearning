using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Communication;
using Learning.Shared.Application.Helpers;
using Learning.Shared.Common.Dto;
using Learning.Shared.Constants;
using MediatR;

namespace Learning.Business.Requests.Identity;

public class CreateOtpCommand : IRequest<ResponseDto<long>>
{
    /// <summary>
    /// 10 digit phone number
    /// </summary>
    public required string MobileNumber { get; set; }
}
public class CreateOtpCommandHandler : IRequestHandler<CreateOtpCommand, ResponseDto<long>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ISmsService _smsService;
    public CreateOtpCommandHandler(IAppDbContextFactory dbContext, ISmsService smsService)
    {
        _dbContext = dbContext.CreateDbContext();
        _smsService = smsService;
    }

    public async Task<ResponseDto<long>> Handle(CreateOtpCommand request, CancellationToken cancellationToken)
    {
        var otpNumber = IdentityHelper.GenerateOtp();
        var phoneNumber = IdentityHelper.ToUsername(request.MobileNumber);
        var otp = new Domain.Identity.OtpHistory
        {
            IsUsed = false,
            Otp = otpNumber,
            UserName = phoneNumber
        };

        var smsresult = await _smsService.SendOtpAsync(phoneNumber, otpNumber);
        if (smsresult.Success)
        {
            _dbContext.OtpHistory.Add(otp);
            await _dbContext.SaveAsync(cancellationToken);
        }
        return new(otp.Id);
    }
}

