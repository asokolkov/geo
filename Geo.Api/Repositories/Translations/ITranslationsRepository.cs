using Geo.Api.Domain.Translations;

namespace Geo.Api.Repositories.Translations;

internal interface ITranslationsRepository
{
    public Task<Translation?> GetAsync(int entityId, string type, string languageId, string value,
        CancellationToken cancellationToken = default);

    public Task<Translation> CreateAsync(int entityId, string type, string languageId, string value,
        CancellationToken cancellationToken = default);
    
    public Task<Translation> UpdateAsync(int entityId, string type, string languageId, string value,
        CancellationToken cancellationToken = default);
}