using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class RailwaysService : CronJobService
{
    private readonly IExtractor<Railway> extractor;

    public RailwaysService(IScheduleConfig<RegionsService> config, IExtractor<Railway> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.extractor = extractor;
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var railways = await extractor.Extract();
        // TODO: send to API client
    }
}