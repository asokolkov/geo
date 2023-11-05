namespace ObjectsLoader.Clients;

public interface ITranslatorClient
{
    public Task<string?> Fetch(string text, string target, string? source = null);
}