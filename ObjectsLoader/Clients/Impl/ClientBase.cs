using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Clients.Impl;

public abstract class ClientBase
{
    protected readonly HttpClient Client = new();
    protected readonly ILogger<ClientBase> Logger;

    protected ClientBase(ILogger<ClientBase> logger)
    {
        Logger = logger;
    }

    protected async Task<HttpResponseMessage?> SendRequest(string query)
    {
        Logger.LogInformation("Trying to send request");
        try
        {
            var response = await Client.GetAsync(query);
            Logger.LogInformation("Request sent without exceptions, returning response");
            return response;
        }
        catch (HttpRequestException _)
        {
            Logger.LogInformation("Got exception on request with query: '{Query}', returning null", query);
            return null;
        }
    }
}