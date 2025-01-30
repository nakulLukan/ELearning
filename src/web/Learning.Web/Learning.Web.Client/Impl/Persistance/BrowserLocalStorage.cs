using Blazored.LocalStorage;
using Learning.Web.Client.Contracts.Persistance;

namespace Learning.Web.Client.Impl.Persistance;

public class BrowserLocalStorage : IBrowserStorage
{
    readonly ILocalStorageService _localStorage;

    public BrowserLocalStorage(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async ValueTask<bool> HasKey(string key)
    {
        return await _localStorage.ContainKeyAsync(key);
    }

    public async ValueTask<T?> Get<T>(string key, string encryptionKey)
    {
        var encryptedData = await _localStorage.GetItemAsync<string>(key);
        return System.Text.Json.JsonSerializer.Deserialize<T?>(encryptedData!);
    }

    public async ValueTask Set<T>(string key, string encryptionKey, T data)
    {
        await _localStorage.SetItemAsync(key, System.Text.Json.JsonSerializer.Serialize(data));
    }
}
