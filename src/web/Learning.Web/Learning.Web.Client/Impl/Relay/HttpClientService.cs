
using Learning.Web.Client.Contracts.Relay;
using System.Net.Http.Json;

namespace Learning.Web.Client.Impl.Relay;
public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetAsync<T>(string uri)
    {
        var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await ParseResponse<T>(response);
    }

    public async Task<T> PostAsync<T>(string uri, object data)
    {
        var response = await _httpClient.PostAsJsonAsync(uri, data);
        response.EnsureSuccessStatusCode();
        return await ParseResponse<T>(response);
    }

    public async Task<T> PutAsync<T>(string uri, object data)
    {
        var response = await _httpClient.PutAsJsonAsync(uri, data);
        response.EnsureSuccessStatusCode();
        return await ParseResponse<T>(response);
    }

    public async Task DeleteAsync(string uri)
    {
        var response = await _httpClient.DeleteAsync(uri);
        response.EnsureSuccessStatusCode();
    }

    private static async Task<T> ParseResponse<T>(HttpResponseMessage response)
    {
        return await response.Content.ReadFromJsonAsync<T>() ?? throw new ArgumentNullException("Response cannot be null");
    }

}
