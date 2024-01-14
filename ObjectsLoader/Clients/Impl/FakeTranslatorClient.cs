using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Clients.Impl;

public class FakeTranslatorClient : ITranslatorClient
{
    private readonly ILogger<FakeTranslatorClient> logger;
    
    public FakeTranslatorClient(ILogger<FakeTranslatorClient> logger)
    {
        this.logger = logger;
        this.logger.LogError("FakeTranslatorClient initialized");
    }
    
    public Task<string?> Fetch(string text, string target, string? source = null)
    {
        logger.LogWarning("Sending query from fake translator, returning text: {Text} without translation", text);
        return Task.FromResult(text);
    }
}