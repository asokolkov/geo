using Geo.Api.Repositories.Translations.Models;

namespace Geo.Api.Repositories.Translations;

internal interface ITranslationsRepository<TTranslationEntity>
    where TTranslationEntity : TranslationEntity
{
    public Task<TTranslationEntity?> GetAsync(int entityId, string languageId,
        CancellationToken cancellationToken = default);

    public Task<TTranslationEntity> CreateAsync(int entityId, string languageId, string value,
        CancellationToken cancellationToken = default);

    public Task<TTranslationEntity> UpdateAsync(int entityId, string languageId, string value,
        CancellationToken cancellationToken = default);
}