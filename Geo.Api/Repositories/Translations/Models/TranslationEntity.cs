namespace Geo.Api.Repositories.Translations.Models;

internal sealed class TranslationEntity
{
    public TranslationEntity(int entityId, string type, string languageId, string value, DateTimeOffset updatedAt)
    {
        EntityId = entityId;
        Type = type;
        LanguageId = languageId;
        Value = value;
        UpdatedAt = updatedAt;
    }

#pragma warning disable CS8618
    private TranslationEntity()
    {
    }
#pragma warning restore CS8618
    
    public int EntityId { get; }
    
    public string Type { get; }
    
    public string LanguageId { get; }
    
    public string Value { get; set; }
    
    public DateTimeOffset UpdatedAt { get; set; }
}