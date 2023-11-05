using AutoMapper;
using Geo.Api.Domain.TranslationLanguage;
using Geo.Api.Repositories.TranslationLanguages.Models;
using Microsoft.EntityFrameworkCore;

namespace Geo.Api.Repositories.TranslationLanguages;

internal sealed class TranslationLanguagesRepository : ITranslationLanguagesRepository
{
    private readonly ApplicationContext context;
    private readonly IMapper mapper;

    public TranslationLanguagesRepository(ApplicationContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<TranslationLanguage?> GetAsync(string language, CancellationToken cancellationToken = default)
    {
        var regionEntity = await context.TranslationLanguages
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Language == language, cancellationToken);

        return mapper.Map<TranslationLanguage>(regionEntity);
    }

    public async Task<TranslationLanguage> CreateAsync(string language, string description, CancellationToken cancellationToken = default)
    {
        var newLanguage = new TranslationLanguageEntity(language, description);
        var entry = await context.AddAsync(newLanguage, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<TranslationLanguage>(entry.Entity);
    }
}