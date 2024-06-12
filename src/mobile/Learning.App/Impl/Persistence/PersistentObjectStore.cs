using Learning.Core.Contracts.Persistence;
using Newtonsoft.Json;

namespace Learning.App.Impl.Persistence;

public class PersistentObjectStore : IPersistentObjectStore
{
    private readonly IPersistenceService _persistenceService;

    public PersistentObjectStore(IPersistenceService persistenceService)
    {
        _persistenceService = persistenceService;
    }

    public T GetObject<T>(string key)
    {
        if (_persistenceService.ContainsKey(key) == false)
            throw new KeyNotFoundException();

        var jsonStr = _persistenceService.Get(key, "");
        return JsonConvert.DeserializeObject<T>(jsonStr);
    }

    public void InsertObject<T>(string key, T value)
    {
        var jsonStr = JsonConvert.SerializeObject(value);
        _persistenceService.Set(key, jsonStr);
    }

    public void InvalidateObject<T>(string key)
    {
        _persistenceService.Remove(key);
    }

    public bool HasObject<T>(string key)
    {
        return _persistenceService.ContainsKey(key);
    }

    public void InvalidateAll()
    {
        _persistenceService.Clear();
    }
}