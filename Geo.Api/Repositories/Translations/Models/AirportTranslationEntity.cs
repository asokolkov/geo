using Geo.Api.Repositories.Airports.Models;

namespace Geo.Api.Repositories.Translations.Models;

internal sealed class AirportTranslationEntity : TranslationEntity
{
    public AirportTranslationEntity(int entityId, string languageId, string translation,
        DateTimeOffset updatedAt) : base(entityId, languageId, translation, updatedAt)
    {
    }

    public AirportTranslationEntity()
    {
    }
    
    public AirportEntity? AirportEntity { get; init; } 
}