namespace Learning.Web.Client.Contracts.Relay;
public interface IHttpClientService
{
    Task<T> GetAsync<T>(string uri);
    Task<T> PostAsync<T>(string uri, object data);
    Task<T> PutAsync<T>(string uri, object data);
    Task DeleteAsync(string uri);
}