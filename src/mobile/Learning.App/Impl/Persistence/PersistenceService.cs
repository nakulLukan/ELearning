using Learning.Core.Contracts.Persistence;

namespace Learning.App.Impl.Persistence;

public class PersistenceService : IPersistenceService
{
    public void Set(string key, bool value)
    {
        Preferences.Set(key, value);
    }

    public void Set(string key, double value)
    {
        Preferences.Set(key, value);
    }

    public void Set(string key, int value)
    {
        Preferences.Set(key, value);
    }

    public void Set(string key, float value)
    {
        Preferences.Set(key, value);
    }

    public void Set(string key, long value)
    {
        Preferences.Set(key, value);
    }

    public void Set(string key, string value)
    {
        Preferences.Set(key, value);
    }

    public void Set(string key, DateTime value)
    {
        Preferences.Set(key, value);
    }

    public bool Get(string key, bool value)
    {
        return Preferences.Get(key, value);
    }

    public double Get(string key, double value)
    {
        return Preferences.Get(key, value);
    }

    public int Get(string key, int value)
    {
        return Preferences.Get(key, value);
    }

    public float Get(string key, float value)
    {
        return Preferences.Get(key, value);
    }

    public long Get(string key, long value)
    {
        return Preferences.Get(key, value);
    }

    public string Get(string key, string value)
    {
        return Preferences.Get(key, value);
    }

    public DateTime Get(string key, DateTime value)
    {
        return Preferences.Get(key, value);
    }

    public bool ContainsKey(string key)
    {
        return Preferences.ContainsKey(key);
    }

    public void Remove(string key)
    {
        Preferences.Remove(key);
    }

    public void Clear()
    {
        Preferences.Clear();
    }
}