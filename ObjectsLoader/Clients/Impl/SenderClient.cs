using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ObjectsLoader.JsonModels;

namespace ObjectsLoader.Clients.Impl;

public class SenderClient : ISenderClient
{
    private const string ApiUrl = "https://d5d59k2qaq07jqbkm97l.apigw.yandexcloud.net";
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
        var payload = JsonSerializer.Serialize(model);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        
        HttpResponseMessage? response;
        try
        {
            response = await client.PostAsync(route, content);
        }
        catch (HttpRequestException _)
        {
            response = null;
        }

        if (response is null || response.StatusCode != HttpStatusCode.OK)
        {
            return default;
        }
        
        var stringResponse = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonResponse<T>>(stringResponse);
        return jsonElement is null ? default : jsonElement.Model;
    }
}