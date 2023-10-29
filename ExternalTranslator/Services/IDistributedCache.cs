using ExternalTranslator.JsonModels;

namespace ExternalTranslator.Services;

public interface IDistributedCache
{
    public TranslatorJson Get(string key);
    public Task<TranslatorJson> GetAsync(string key);
    public void Set(TranslatorJson translator);
    public Task SetAsync(TranslatorJson translator);
}