namespace Learning.Core.Contracts.Persistence;

public interface ICacheObjectStore
{
    void Start(string storeName);
    Task<T?> GetObjectAsync<T>(string key);
    Task<(bool HasData, T? Data)> TryGetObjectAsync<T>(string key);
    Task InsertObjectAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration = null);
    Task InvalidateObjectAsync<T>(string key);
    Task<bool> HasObjectAsync<T>(string key);
    Task InvalidateAllAsync();
    void Flush();
}