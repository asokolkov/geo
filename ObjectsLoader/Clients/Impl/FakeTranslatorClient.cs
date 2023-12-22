using Microsoft.Extensions.Logging;

namespace ObjectsLoader.Clients.Impl;

public class FakeTranslatorClient : ClientBase, ITranslatorClient
{
    public FakeTranslatorClient(ILogger<FakeTranslatorClient> logger) : base(logger)
    {
        Logger.LogWarning("FakeTranslatorClient initialized");
    }
    
    public Task<string?> Fetch(string text, string target, string? source = null)
    {
        Logger.LogWarning("Sending query from fake translator, returning text: {Text} without translation", text);
        return Task.FromResult(text);
    }
}