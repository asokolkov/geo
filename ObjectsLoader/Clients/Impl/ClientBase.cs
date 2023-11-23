namespace ObjectsLoader.Clients.Impl;

public abstract class ClientBase
{
    protected readonly HttpClient Client = new();

    protected async Task<HttpResponseMessage?> SendRequest(string query)
    {
        try
        {
            var response = await Client.GetAsync(query);
            return response;
        }
        catch (HttpRequestException _)
        {
            return null;
        }
    }
}