namespace Learning.Web.Client.Contracts.Persistance;

public interface IBrowserStorage
{
    public ValueTask<T?> Get<T>(string key, string encryptionKey);
    public ValueTask Set<T>(string key, string encryptionKey, T data);
}
