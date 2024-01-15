using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Clients.Impl;

public class SenderClient : ISenderClient
{
    private const string ApiUrl = "https://localhost:8080";
    private readonly ILogger<SenderClient> logger;
    private readonly HttpClient client = new();

    public SenderClient(ILogger<SenderClient> logger)
    {
        this.logger = logger;
        client.BaseAddress = new Uri(ApiUrl);
        this.logger.LogInformation("Sender client initialized");
    }
    
    public async Task<T?> Send<T>(string route, T model)
    {
        HttpResponseMessage? response;
        try
        {
            response = await client.PostAsJsonAsync(route, model);
        }
        catch (HttpRequestException _)
        {
            response = null;
        }

        if (response is null || response.StatusCode != HttpStatusCode.Created)
        {
            return default;
        }

        return default;
    }
}