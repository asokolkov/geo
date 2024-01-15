using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Clients;

public interface INominatimClient
{
    public Task<string?> Fetch(string key, double latitude, double longitude);
    public Task<string?> Fetch(string query);
}