using FluentResults;
using Learning.Business.Requests.DataCollection.ContactUsRequest;
using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.DataCollection.ContactUsRequest;
using Learning.Web.Client.Contracts.Services.DataCollection;
using MediatR;

namespace Learning.Web.Services.DataCollection;

public class ContactUsDataService : IContactUsDataService
{
    private readonly IMediator _mediator;
    public ContactUsDataService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<ResponseDto<long>>> AddContactInformation(AddContactInformationCommandRequestDto request)
    {
        var data = await _mediator.Send(new AddContactInformationCommand()
        {
            ContactNumber = request.ContactNumber,
            CountryCode = request.CountryCode,
            Name = request.Name,
            EmailAddress = request.EmailAddress,
        });
        return data;
    }
}
