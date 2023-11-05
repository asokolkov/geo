namespace ObjectsLoader.Clients;

public interface INominatimClient
{
    public Task<string?> Fetch(double latitude, double longitude);
}