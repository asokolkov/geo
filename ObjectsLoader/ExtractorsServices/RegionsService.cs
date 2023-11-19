using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class RegionsService : CronJobService
{
    private readonly IExtractor<Region> extractor;

    public RegionsService(IScheduleConfig<RegionsService> config, IExtractor<Region> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.extractor = extractor;
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var regions = await extractor.Extract();
        // TODO: send to API client
    }
}