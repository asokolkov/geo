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
        logger.LogInformation("{{method=\"countries_service_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }

    public override async Task DoWork(CancellationToken cancellationToken)
    {
        var countries = await extractor.Extract();
        var amount = countries.Count;
        if (amount == 0)
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"No countries extracted\"}}");
        }
        else
        {
            logger.LogInformation("{{method=\"do_work\" msg=\"Found {Amount} countries\"}}", amount);
        }
        
        // TODO: send to API client
        
        logger.LogInformation("{{method=\"do_work\" status=\"success\" msg=\"All countries sent to API\"}}");
    }
}