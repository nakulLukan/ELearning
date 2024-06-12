namespace Learning.Core.Contracts.Persistence;

public interface IPersistentObjectStore
{
    T GetObject<T>(string key);
    void InsertObject<T>(string key, T value);
    void InvalidateObject<T>(string key);
    bool HasObject<T>(string key);
    void InvalidateAll();
}