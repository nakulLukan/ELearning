namespace Learning.Business.Contracts.Persistence;

public interface IAppCache
{
    public (bool HasData, T? Data) Get<T>(string key);

    public T Set<T>(string key, T Data);
    public T SetWithSlidingExpiration<T>(string key, T Data, TimeSpan expiry);

    public void DeleteKey(string key);

}
