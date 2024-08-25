
using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Learning.Web.Client.Contracts.Persistance;

namespace Learning.Web.Client.Impl.Persistance;

public class BrowserLocalStorage : IBrowserStorage
{
    readonly ILocalStorageService _localStorage;
    readonly ICryptoService _cryptoService;

    public BrowserLocalStorage(ILocalStorageService localStorage,
                               ICryptoService cryptoService)
    {
        _localStorage = localStorage;
        _cryptoService = cryptoService;
    }

    public async ValueTask<T?> Get<T>(string key, string encryptionKey)
    {
        var encryptedData = await _localStorage.GetItemAsync<string>(key);
        var decryptedData = await _cryptoService.DecryptAsync(encryptedData ?? throw new ArgumentNullException(nameof(key)));
        return System.Text.Json.JsonSerializer.Deserialize<T?>(decryptedData);
    }

    public async ValueTask Set<T>(string key, string encryptionKey, T data)
    {
        var encryptedData = await _cryptoService.EncryptAsync(System.Text.Json.JsonSerializer.Serialize(data));
        await _localStorage.SetItemAsync(key, encryptedData.Value);
    }
}
