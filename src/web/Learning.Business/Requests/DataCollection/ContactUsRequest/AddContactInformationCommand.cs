﻿using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.DataCollection.ContactUsRequest;
using MediatR;

namespace Learning.Business.Requests.DataCollection.ContactUsRequest;

public class AddContactInformationCommand : AddContactInformationCommandRequestDto, IRequest<ResponseDto<long>>
{
}
public class AddContactInformationCommandHandler : IRequestHandler<AddContactInformationCommand, ResponseDto<long>>
{
    private readonly IAppDbContext _appDbContext;

    public AddContactInformationCommandHandler(IAppDbContextFactory appDbContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
    }

    public async Task<ResponseDto<long>> Handle(AddContactInformationCommand request, CancellationToken cancellationToken)
    {
        var contactUs = new Domain.Identity.ContactUsRequest()
        {
            Id = 0,
            CountryCode = request.CountryCode,
            Name = request.Name.ToHumanName(),
            PhoneNumber = request.ContactNumber.Trim(),
            CreatedOn = AppDateTime.UtcNow,
            EmailAddress = request.EmailAddress,
            LastUpdatedOn = AppDateTime.UtcNow,
        };

        _appDbContext.ContactUsRequests.Add(contactUs);
        await _appDbContext.SaveAsync(cancellationToken);
        return new(contactUs.Id);
    }
}


