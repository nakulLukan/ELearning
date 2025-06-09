using Learning.Business.Dto.Identity;
using Learning.Business.Impl.Data;
using Learning.Domain.Identity;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Application.Helpers;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Learning.Business.Requests.Identity;

public class VerifyOtpCommand : IRequest<VerifyOtpResponseDto>
{
    public required string MobileNumber { get; set; }
    public required int Otp { get; set; }
}
public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, VerifyOtpResponseDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IExternalIdentityProvider _identityProvider;
    private readonly bool _isTestMode;
    public VerifyOtpCommandHandler(
        IAppDbContextFactory dbContext,
        IExternalIdentityProvider identityProvider,
        IConfiguration configuration)
    {
        _dbContext = dbContext.CreateDbContext();
        _identityProvider = identityProvider;
        _isTestMode = bool.Parse(configuration[AppSettingsKeyConstant.Sms_TestMode]!);
    }

    public async Task<VerifyOtpResponseDto> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        // Check if any otp history exists which is not used for given requestid
        var username = IdentityHelper.ToUsername(request.MobileNumber);
        var updateCount = await _dbContext.OtpHistory
            .Where(x => x.UserName == username
                && x.Otp == request.Otp
                && x.ExpiresOn >= AppDateTime.UtcNow)
            .ExecuteDeleteAsync(cancellationToken);
        if (updateCount == 1 || _isTestMode)
        {
            await _identityProvider.ConfirmPhoneNumber(request.MobileNumber);
            await AddUserToDatabase(request.MobileNumber, cancellationToken).ConfigureAwait(false);
            return new VerifyOtpResponseDto()
            {
                Matched = true,
                Username = username
            };
        }

        return new VerifyOtpResponseDto()
        {
            Username = null,
            Matched = false
        };
    }

    private async Task AddUserToDatabase(string username, CancellationToken cancellationToken)
    {
        var userDetails = await _identityProvider.GetUserDetailsByUsername(username);
        var newUser = new ApplicationUser()
        {
            Id = userDetails.Sub,
            AccountCreatedOn = userDetails.CreatedOn,
            IsActive = true,
            IsAdmin = IdentityHelper.IsAdminUser(userDetails.Role),
            OtherDetails = new()
            {
                Email = userDetails.Email,
                PhoneNumber = userDetails.PhoneNumber,
                EmailConfirmed = userDetails.IsEmailConfirmed,
                PhoneNumberConfirmed = userDetails.IsPhoneNumberConfirmed,
                FullName = userDetails.FullName,
                NormalizedEmail = userDetails.Email.ToNormalizedString(),
                Place = userDetails.Place.TrimToLen(DomainConstant.PlaceFieldMaxLength).ToUpper(),
            },
            RoleId = null
        };

        _dbContext.AspNetUsers.Add(newUser);
        await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
    }
}

