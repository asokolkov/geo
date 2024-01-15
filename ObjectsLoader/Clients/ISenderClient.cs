namespace ObjectsLoader.Clients;

public interface ISenderClient
{
    public Task<T?> Send<T>(string route, T model);
}