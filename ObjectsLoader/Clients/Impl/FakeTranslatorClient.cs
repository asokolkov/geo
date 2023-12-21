namespace ObjectsLoader.Clients.Impl;

public class FakeTranslatorClient : ITranslatorClient
{
    public Task<string?> Fetch(string text, string target, string? source = null)
    {
        return Task.FromResult(text);
    }
}