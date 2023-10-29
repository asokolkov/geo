namespace ExternalTranslator.Services.Translators;

public interface ITranslatorClient
{
    public Task TryResetModel();
    public bool CanTranslate(string text);
    public Task<string?> Translate(string text, string? source, string target);
}