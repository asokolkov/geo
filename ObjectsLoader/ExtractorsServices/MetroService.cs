using Microsoft.Extensions.Logging;
using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class MetroService : CronJobService
{
    private readonly ILogger<MetroService> logger;
    private readonly IExtractor<Metro> extractor;
    
    public MetroService(ILogger<MetroService> logger, IScheduleConfig<AirportsService> config, IExtractor<Metro> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.logger = logger;
        this.extractor = extractor;
        logger.LogInformation("MetroService initialized");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting cron task with metro stations extraction");
        var metro = await extractor.Extract();
        var amount = metro.Count;
        if (amount == 0)
        {
            logger.LogInformation("Found 0 metro stations, nothing to send to API, finishing");
            return;
        }

        logger.LogInformation("Found {Amount} metro stations, sending to API", amount);

        // TODO: send to API client
        
        logger.LogInformation("All metro stations sent to API");
    }
}