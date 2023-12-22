using Microsoft.Extensions.Logging;
using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class AirportsService : CronJobService
{
    private readonly ILogger<AirportsService> logger;
    private readonly IExtractor<Airport> extractor;

    public AirportsService(ILogger<AirportsService> logger, IScheduleConfig<AirportsService> config, IExtractor<Airport> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.logger = logger;
        this.extractor = extractor;
        logger.LogInformation("AirportsService initialized");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting cron task with airports extraction");
        var airports = await extractor.Extract();
        var amount = airports.Count;
        if (amount == 0)
        {
            logger.LogInformation("Found 0 airports, nothing to send to API, finishing");
            return;
        }

        logger.LogInformation("Found {Amount} airports, sending to API", amount);

        // TODO: send to API client
        
        logger.LogInformation("All airports sent to API");
    }
}