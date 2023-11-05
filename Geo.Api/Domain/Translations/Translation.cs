namespace Geo.Api.Domain.Translations;

internal sealed class Translation
{
    public Translation(int entityId, string type, string languageId, string value, DateTimeOffset updatedAt)
    {
        EntityId = entityId;
        Type = type;
        LanguageId = languageId;
        Value = value;
        UpdatedAt = updatedAt;
    }

    public int EntityId { get; }
    
    public string Type { get; }
    
    public string LanguageId { get; }
    
    public string Value { get; }
    
    public DateTimeOffset UpdatedAt { get; }
}