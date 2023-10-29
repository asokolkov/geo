namespace ExternalTranslator.Translators;

public interface ITranslatorClient
{
    public Task TryResetRestrictions();
    public bool CanTranslate(string text);
    public Task<string?> Translate(string text, string? source, string target);
}