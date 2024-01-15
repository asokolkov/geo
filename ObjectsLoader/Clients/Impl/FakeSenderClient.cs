namespace ObjectsLoader.Clients.Impl;

public class FakeSenderClient : ISenderClient
{
    public async Task<T?> Send<T>(string route, T model)
    {
        return model;
    }
}