using System.Net;
using System.Text.Json;
using Learning.Shared.Common.Utilities;
using Refit;

namespace Learning.Web.Client.Utilities.RestClient;

public class ProblemDetailsHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                throw new AppApiException((HttpStatusCode)problemDetails!.Status, problemDetails.Detail!, problemDetails.Title!);
            }
            catch (JsonException)
            {
                throw new HttpRequestException("Unexpected error format");
            }
        }

        return response;
    }
}
