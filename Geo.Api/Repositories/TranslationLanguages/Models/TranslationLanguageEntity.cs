namespace Geo.Api.Repositories.TranslationLanguages.Models;

internal sealed class TranslationLanguageEntity
{
    public TranslationLanguageEntity(string language, string description)
    {
        Language = language;
        Description = description;
    }
    
#pragma warning disable CS8618
    private TranslationLanguageEntity()
    {
    }
#pragma warning restore CS8618

    public string Language { get; }
    
    public string Description { get; }
}