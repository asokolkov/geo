using Geo.Api.Domain.TranslationLanguage;

namespace Geo.Api.Repositories.TranslationLanguages;

internal interface ITranslationLanguagesRepository
{
    public Task<TranslationLanguage?> GetAsync(string language, CancellationToken cancellationToken = default);

    public Task<TranslationLanguage> CreateAsync(string language, string description,
        CancellationToken cancellationToken = default);
}