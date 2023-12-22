using Microsoft.Extensions.Logging;
using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class RailwaysService : CronJobService
{
    private readonly ILogger<RailwaysService> logger;
    private readonly IExtractor<Railway> extractor;

    public RailwaysService(ILogger<RailwaysService> logger, IScheduleConfig<RegionsService> config, IExtractor<Railway> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.logger = logger;
        this.extractor = extractor;
        logger.LogInformation("RailwaysService initialized");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting cron task with railways extraction");
        var railways = await extractor.Extract();
        var amount = railways.Count;
        if (amount == 0)
        {
            logger.LogInformation("Found 0 railways, nothing to send to API, finishing");
            return;
        }
        
        logger.LogInformation("Found {Amount} railways, sending to API", amount);

        // TODO: send to API client
        
        logger.LogInformation("All railways sent to API");
    }
}