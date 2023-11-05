using AutoMapper;
using Geo.Api.Domain.Regions;
using Geo.Api.Domain.Translations;
using Geo.Api.Repositories.Exceptions;
using Geo.Api.Repositories.Translations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Geo.Api.Repositories.Translations;

internal sealed class TranslationsRepository : ITranslationsRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly IMapper mapper;
    private readonly ISystemClock systemClock;

    public TranslationsRepository(ILogger<TranslationsRepository> logger, ApplicationContext context, IMapper mapper,
        ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.mapper = mapper;
        this.systemClock = systemClock;
    }

    public async Task<Translation?> GetAsync(int entityId, string type, string languageId, string value,
        CancellationToken cancellationToken = default)
    {
        var regionEntity = await context.Translations
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntityId == entityId && e.Type == type && e.LanguageId == languageId,
                cancellationToken);

        return mapper.Map<Translation>(regionEntity);
    }

    public async Task<Translation> CreateAsync(int entityId, string type, string languageId, string value,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var newTranslation = new TranslationEntity(entityId, type, languageId, value, now);
        var entry = await context.AddAsync(newTranslation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Translation>(entry.Entity);
    }

    public async Task<Translation> UpdateAsync(int entityId, string type, string languageId, string value,
        CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var regionEntity = await context.Translations
            .FirstOrDefaultAsync(e => e.EntityId == entityId && e.Type == type && e.LanguageId == languageId,
                cancellationToken);

        if (regionEntity is null)
        {
            logger.LogWarning("Translation with type='{EntityType}' id='{EntityId}' language='{LanguageId}' not found",
                type, entityId, languageId);
            throw new EntityNotFoundException(typeof(Translation));
        }

        regionEntity.Value = value;
        regionEntity.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Translation>(regionEntity);
    }
}