using Learning.Business.Impl.Data;
using Learning.Domain.Identity;
using Learning.Shared.Application.Contracts.Communication;
using Learning.Shared.Application.Helpers;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

    private const int OTP_EXPIRY_IN_MINUTES = 10;
    private const int NEXT_OTP_AFTER_IN_MINUTES = 2;
    private const int MAX_OTP_REQUEST_PER_DAY = 10;
    public CreateOtpCommandHandler(IAppDbContextFactory dbContext, ISmsService smsService)
    {
        _dbContext = dbContext.CreateDbContext();
        _smsService = smsService;
    }

    public async Task<ResponseDto<long>> Handle(CreateOtpCommand request, CancellationToken cancellationToken)
    {
        var otpNumber = IdentityHelper.GenerateOtp();
        var phoneNumber = IdentityHelper.ToUsername(request.MobileNumber);
        var existingOtp = await _dbContext.OtpHistory
            .Where(x => x.UserName == phoneNumber)
            .FirstOrDefaultAsync(cancellationToken);

        // Create otp if its for first time.
        // If not first time, then check when last otp was issued.
        //      If otp was issued within 2 mins then do not send any otp
        //      If otp was issued more than 10 times then do not send any otp
        //      If no otps are issued today, then send new otp.
        if (existingOtp == null)
        {
            return await AddNewOtpEntry(otpNumber, phoneNumber, cancellationToken);
        }
        else
        {
            return await UpdateExistingOtpEntry(otpNumber, phoneNumber, existingOtp, cancellationToken);
        }
    }

    private async Task<ResponseDto<long>> UpdateExistingOtpEntry(int otpNumber, string phoneNumber, OtpHistory? existingOtp, CancellationToken cancellationToken)
    {
        // If otp is created for first time then allow to resend otp
        // Or last issued otp was 2 minutes before
        if (existingOtp!.NextOtpAfter == null
            || existingOtp.TimesRequested < MAX_OTP_REQUEST_PER_DAY && existingOtp.NextOtpAfter.Value <= DateTimeOffset.UtcNow)
        {
            await _dbContext.OtpHistory.Where(x => x.UserName == phoneNumber)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(p => p.NextOtpAfter, DateTimeOffset.UtcNow.AddMinutes(NEXT_OTP_AFTER_IN_MINUTES))
                    .SetProperty(p => p.TimesRequested, existingOtp.TimesRequested + 1)
                    .SetProperty(p => p.Otp, otpNumber)
                    .SetProperty(p => p.ExpiresOn, AppDateTime.UtcNow.AddMinutes(OTP_EXPIRY_IN_MINUTES)), cancellationToken);
        }
        else if (existingOtp.TimesRequested >= MAX_OTP_REQUEST_PER_DAY && existingOtp.NextOtpAfter.Value.Date < DateTimeOffset.UtcNow.Date)
        {
            // If otp requested has reached maximum and if no otp issued today then send otp
            await _dbContext.OtpHistory.Where(x => x.UserName == phoneNumber)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(p => p.NextOtpAfter, default(DateTimeOffset?))
                    .SetProperty(p => p.TimesRequested, 0)
                    .SetProperty(p => p.Otp, otpNumber)
                    .SetProperty(p => p.ExpiresOn, AppDateTime.UtcNow.AddMinutes(OTP_EXPIRY_IN_MINUTES)), cancellationToken);
        }
        else if (DateTimeOffset.UtcNow <= existingOtp.NextOtpAfter || existingOtp.TimesRequested >= MAX_OTP_REQUEST_PER_DAY)
        {
            // Do not send any otp if user has reached maximum otp request or last issued otp was before 2 mins.
            return new(0);
        }
        else
        {
            throw new NotImplementedException();
        }

        var smsresult = await _smsService.SendOtpAsync(phoneNumber, otpNumber);
        return new(existingOtp.Id);
    }

    private async Task<ResponseDto<long>> AddNewOtpEntry(int otpNumber, string phoneNumber, CancellationToken cancellationToken)
    {
        var newOtp = new Domain.Identity.OtpHistory
        {
            Otp = otpNumber,
            UserName = phoneNumber,
            TimesRequested = 0,
            NextOtpAfter = null,
            ExpiresOn = AppDateTime.UtcNow.AddMinutes(OTP_EXPIRY_IN_MINUTES)
        };

        var smsresult = await _smsService.SendOtpAsync(phoneNumber, otpNumber);
        if (smsresult.Success)
        {
            _dbContext.OtpHistory.Add(newOtp);
            await _dbContext.SaveAsync(cancellationToken);
        }
        return new(newOtp.Id);
    }
}

