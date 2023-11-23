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
        logger.LogInformation("{{method=\"railways_service_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var railways = await extractor.Extract();
        var amount = railways.Count;
        if (amount == 0)
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"No railways extracted\"}}");
        }
        else
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"Found {Amount} railways\"}}", amount);
        }
        
        // TODO: send to API client
        
        logger.LogInformation("{{method=\"do_work\" status=\"success\" msg=\"All railways sent to API\"}}");
    }
}