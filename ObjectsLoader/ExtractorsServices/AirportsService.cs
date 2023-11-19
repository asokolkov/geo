using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class AirportsService : CronJobService
{
    private readonly IExtractor<Airport> extractor;

    public AirportsService(IScheduleConfig<AirportsService> config, IExtractor<Airport> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.extractor = extractor;
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var airports = await extractor.Extract();
        // TODO: send to API client
    }
}