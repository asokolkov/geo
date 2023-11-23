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
        logger.LogInformation("{{method=\"cities_service_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var cities = await extractor.Extract();
        var amount = cities.Count;
        if (amount == 0)
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"No cities extracted\"}}");
        }
        else
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"Found {Amount} cities\"}}", amount);
        }
        
        // TODO: send to API client
        
        logger.LogInformation("{{method=\"do_work\" status=\"success\" msg=\"All cities sent to API\"}}");
    }
}