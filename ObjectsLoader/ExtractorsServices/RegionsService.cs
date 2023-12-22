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
        logger.LogInformation("RegionsService initialized");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting cron task with regions extraction");
        var regions = await extractor.Extract();
        var amount = regions.Count;
        if (amount == 0)
        {
            logger.LogInformation("Found 0 regions, nothing to send to API, finishing");
            return;
        }
        else
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"Found {Amount} regions\"}}", amount);
        }
        
        // TODO: send to API client
        
        logger.LogInformation("{{method=\"do_work\" status=\"success\" msg=\"All regions sent to API\"}}");
    }
}