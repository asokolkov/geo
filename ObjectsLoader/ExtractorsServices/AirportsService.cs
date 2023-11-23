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
        logger.LogInformation("{{method=\"airports_service_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var airports = await extractor.Extract();
        var amount = airports.Count;
        if (amount == 0)
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"No airports extracted\"}}");
        }
        else
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"Found {Amount} airports\"}}", amount);
        }
        
        // TODO: send to API client
        
        logger.LogInformation("{{method=\"do_work\" status=\"success\" msg=\"All airports sent to API\"}}");
    }
}