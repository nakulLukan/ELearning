using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.DataCollection.ContactUsRequest;
using Learning.Web.Client.Contracts.Services.DataCollection;
using Learning.Web.Client.Services.WebServices;

namespace Learning.Web.Client.Services.DataCollection;

public class ContactUsRestDataService : IContactUsDataService
{
    private readonly IDataCollectionRestService _httpClient;
    public ContactUsRestDataService(IDataCollectionRestService httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<ResponseDto<long>>> AddContactInformation(AddContactInformationCommandRequestDto request)
    {
        try
        {
            var result = await _httpClient.AddContactInformation(request);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
