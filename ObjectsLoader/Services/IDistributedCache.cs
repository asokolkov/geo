namespace ObjectsLoader.Services;

public interface IDistributedCache
{
    public string? Get(string key);
    public Task<string?> GetAsync(string key);
    public void Set(string key, string value);
    public Task SetAsync(string key, string value);
}