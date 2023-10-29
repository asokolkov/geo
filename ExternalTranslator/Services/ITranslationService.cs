namespace ExternalTranslator.Services;

public interface ITranslationService
{
    public Task<string?> Translate(string text, string? source, string target);
}