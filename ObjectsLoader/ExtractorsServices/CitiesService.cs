using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class CitiesService : CronJobService
{
    private readonly IExtractor<City> extractor;

    public CitiesService(IScheduleConfig<CitiesService> config, IExtractor<City> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.extractor = extractor;
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var cities = await extractor.Extract();
        // TODO: send to API client
    }
}