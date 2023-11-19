using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class CountriesService : CronJobService
{
    private readonly IExtractor<Country> extractor;

    public CountriesService(IScheduleConfig<CountriesService> config, IExtractor<Country> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.extractor = extractor;
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var countries = await extractor.Extract();
        // TODO: send to API client
    }
}