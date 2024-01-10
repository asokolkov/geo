using AutoMapper;
using Geo.Api.Domain.Regions;
using Geo.Api.Domain.Translations;
using Geo.Api.Repositories.Exceptions;
using Geo.Api.Repositories.Translations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Geo.Api.Repositories.Translations;

internal sealed class AirportTranslationsRepository : ITranslationsRepository<AirportTranslationEntity>
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;

    public AirportTranslationsRepository(ILogger<AirportTranslationsRepository> logger, ApplicationContext context,
        IMapper mapper,
        ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
    }

    public async Task<AirportTranslationEntity?> GetAsync(int entityId, string languageId,
        CancellationToken cancellationToken = default)
    {
        var translationEntity = await context.AirportTranslations
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntityId == entityId && e.LanguageId == languageId,
                cancellationToken);

        return translationEntity;
    }

    public async Task<AirportTranslationEntity> CreateAsync(int entityId, string languageId, string value,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var newTranslation = new AirportTranslationEntity(entityId, languageId, value, now);
        var entry = await context.AddAsync(newTranslation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<AirportTranslationEntity> UpdateAsync(int entityId, string languageId, string translation,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var translationEntity = await context.AirportTranslations
            .FirstOrDefaultAsync(e => e.EntityId == entityId && e.LanguageId == languageId,
                cancellationToken);

        if (translationEntity is null)
        {
            logger.LogWarning("Translation with id='{EntityId}' language='{LanguageId}' not found", entityId,
                languageId);
            throw new EntityNotFoundException(typeof(Translation));
        }

        translationEntity.Translation = translation;
        translationEntity.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        return translationEntity;
    }
}