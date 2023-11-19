namespace ObjectsLoader.Clients;

public interface INominatimClient
{
    public Task<string?> Fetch(string key, double latitude, double longitude);
}