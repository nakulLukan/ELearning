using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Application.Helpers;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Users.PublicUser;

public class EnableDisableUserCommand : IRequest<ResponseDto<bool>>
{
    public required string UserId { get; set; }
}

public class EnableDisableUserCommandHandler : IRequestHandler<EnableDisableUserCommand, ResponseDto<bool>>
{
    private readonly IExternalIdentityProvider _identityProvider;
    private readonly IAppDbContext _appDbContext;

    public EnableDisableUserCommandHandler(
        IExternalIdentityProvider identityProvider,
        IAppDbContextFactory appDbContext)
    {
        _identityProvider = identityProvider;
        _appDbContext = appDbContext.CreateDbContext();
    }

    public async Task<ResponseDto<bool>> Handle(EnableDisableUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.AspNetUsers.Where(x => x.Id == request.UserId)
            .Select(x => new
            {
                x.IsActive,
                x.OtherDetails!.PhoneNumber
            })
            .FirstAsync();

        if (user.IsActive)
        {
            await _identityProvider.DisableUser(IdentityHelper.ToMobileNumber(user.PhoneNumber!));
        }
        else
        {
            await _identityProvider.EnableUser(IdentityHelper.ToMobileNumber(user.PhoneNumber!));
        }

        // Update the is active status
        await _appDbContext.AspNetUsers
            .Where(x => x.Id == request.UserId)
            .ExecuteUpdateAsync(x => x.SetProperty(prop => prop.IsActive, !user.IsActive), cancellationToken);

        return new ResponseDto<bool>(!user.IsActive);
    }
}

