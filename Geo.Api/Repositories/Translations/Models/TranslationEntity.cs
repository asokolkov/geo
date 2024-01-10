namespace Geo.Api.Repositories.Translations.Models;

internal abstract class TranslationEntity
{
    protected TranslationEntity(int entityId, string languageId, string translation,
        DateTimeOffset updatedAt)
    {
        EntityId = entityId;
        LanguageId = languageId;
        Translation = translation;
        UpdatedAt = updatedAt;
    }

#pragma warning disable CS8618
    protected TranslationEntity()
    {
    }
#pragma warning restore CS8618

    public int EntityId { get; }

    public string LanguageId { get; }

    public string Translation { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}