using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Application.Helpers;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Learning.Business.Requests.Identity;

public class ChangePasswordCommand : IRequest<ResponseDto<bool>>
{
    public required string MobileNumber { get; set; }
    public required int Otp { get; set; }
    public required string NewPassword { get; set; }
}
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IExternalIdentityProvider _identityProvider;
    private readonly bool _isTestMode;

    public ChangePasswordCommandHandler(IAppDbContextFactory dbContext,
                                        IExternalIdentityProvider identityProvider,
                                        IConfiguration configuration)
    {
        _dbContext = dbContext.CreateDbContext();
        _identityProvider = identityProvider;
        _isTestMode = bool.Parse(configuration[AppSettingsKeyConstant.Sms_TestMode]!);
    }

    public async Task<ResponseDto<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var username = IdentityHelper.ToUsername(request.MobileNumber);
        var updateCount = await _dbContext.OtpHistory
             .Where(x => x.UserName == username
                 && x.Otp == request.Otp
                 && x.ExpiresOn >= AppDateTime.UtcNow)
             .ExecuteDeleteAsync(cancellationToken);
        if (updateCount == 1 || _isTestMode)
        {
            await _identityProvider.ChangeUserPassword(request.MobileNumber, request.NewPassword);
            return new(true);
        }

        return new(false);
    }
}

