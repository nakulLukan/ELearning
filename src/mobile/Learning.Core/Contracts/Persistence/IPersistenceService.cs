namespace Learning.Core.Contracts.Persistence;

public interface IPersistenceService
{
    void Set(string key, bool value);
    void Set(string key, double value);
    void Set(string key, int value);
    void Set(string key, float value);
    void Set(string key, long value);
    void Set(string key, string value);
    void Set(string key, DateTime value);

    bool Get(string key, bool value);
    double Get(string key, double value);
    int Get(string key, int value);
    float Get(string key, float value);
    long Get(string key, long value);
    string Get(string key, string value);
    DateTime Get(string key, DateTime value);

    bool ContainsKey(string key);
    void Remove(string key);
    void Clear();
}