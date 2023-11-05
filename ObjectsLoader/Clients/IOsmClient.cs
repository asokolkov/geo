namespace ObjectsLoader.Clients;

public interface IOsmClient
{
    public Task<string?> Fetch(string data);
}