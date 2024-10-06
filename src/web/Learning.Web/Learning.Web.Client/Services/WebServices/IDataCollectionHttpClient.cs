using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.DataCollection.ContactUsRequest;
using Refit;

namespace Learning.Web.Client.Services.WebServices;

public interface IDataCollectionHttpClient
{
    [Post("/api/data-collection/add-contact-info")]
    public Task<ResponseDto<long>> AddContactInformation([Body] AddContactInformationCommandRequestDto request);
}
