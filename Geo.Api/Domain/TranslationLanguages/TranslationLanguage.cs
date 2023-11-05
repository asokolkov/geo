namespace Geo.Api.Domain.TranslationLanguage;

internal sealed class TranslationLanguage
{
    public TranslationLanguage(string language, string description)
    {
        Language = language;
        Description = description;
    }

    public string Language { get; }
    
    public string Description { get; }
}