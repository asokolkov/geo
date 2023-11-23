using Microsoft.Extensions.Logging;
using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class RegionsService : CronJobService
{
    private readonly ILogger<RegionsService> logger;
    private readonly IExtractor<Region> extractor;

    public RegionsService(ILogger<RegionsService> logger, IScheduleConfig<RegionsService> config, IExtractor<Region> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.logger = logger;
        this.extractor = extractor;
        logger.LogInformation("{{method=\"regions_service_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var regions = await extractor.Extract();
        var amount = regions.Count;
        if (amount == 0)
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"No regions extracted\"}}");
        }
        else
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"Found {Amount} regions\"}}", amount);
        }
        
        // TODO: send to API client
        
        logger.LogInformation("{{method=\"do_work\" status=\"success\" msg=\"All regions sent to API\"}}");
    }
}