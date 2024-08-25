using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.DataCollection.ContactUsRequest;

namespace Learning.Web.Client.Contracts.Services.DataCollection;

public interface IContactUsDataService
{
    Task<Result<ResponseDto<long>>> AddContactInformation(AddContactInformationCommandRequestDto request);
}
