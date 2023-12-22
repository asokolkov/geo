using Microsoft.Extensions.Logging;
using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class CitiesService : CronJobService
{
    private readonly ILogger<CitiesService> logger;
    private readonly IExtractor<City> extractor;

    public CitiesService(ILogger<CitiesService> logger, IScheduleConfig<CitiesService> config, IExtractor<City> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.logger = logger;
        this.extractor = extractor;
        logger.LogInformation("CitiesService initialized");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting cron task with cities extraction");
        var cities = await extractor.Extract();
        var amount = cities.Count;
        if (amount == 0)
        {
            logger.LogInformation("Found 0 cities, nothing to send to API, finishing");
            return;
        }
        
        logger.LogInformation("Found {Amount} cities, sending to API", amount);

        // TODO: send to API client
        
        logger.LogInformation("All cities sent to API");
    }
}