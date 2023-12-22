using Microsoft.Extensions.Logging;
using ObjectsLoader.Extractors;
using ObjectsLoader.Models;
using ObjectsLoader.ScheduledService;

namespace ObjectsLoader.ExtractorsServices;

public class CountriesService : CronJobService
{
    private readonly ILogger<CountriesService> logger;
    private readonly IExtractor<Country> extractor;

    public CountriesService(ILogger<CountriesService> logger, IScheduleConfig<CountriesService> config, IExtractor<Country> extractor) : base(config.CronExpression, config.TimeZoneInfo)
    {
        this.logger = logger;
        this.extractor = extractor;
        logger.LogInformation("CountriesService initialized");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting cron task with countries extraction");
        var countries = await extractor.Extract();
        var amount = countries.Count;
        if (amount == 0)
        {
            logger.LogInformation("Found 0 countries, nothing to send to API, finishing");
            return;
        }
        
        logger.LogInformation("Found {Amount} countries, sending to API", amount);

        // TODO: send to API client
        
        logger.LogInformation("All countries sent to API");
    }
}