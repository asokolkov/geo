using System.Text.RegularExpressions;
using GeoTimeZone;
using Microsoft.Extensions.Logging;
using TimeZoneConverter;

namespace ObjectsLoader.Services.Impl;

public partial class TimezoneManager : ITimezoneManager
{
    private readonly ILogger<TimezoneManager> logger;
    [GeneratedRegex("[a-zA-Z-+_/\\d]*")] private static partial Regex IanaRegex();

    public TimezoneManager(ILogger<TimezoneManager> logger)
    {
        this.logger = logger;
        logger.LogInformation("TimezoneManager initialized");
    }
    
    public int? GetUtcOffset(double latitude, double longitude)
    {
        logger.LogInformation("Searching utc timezone by latitude: {Lat} and longitude: {Lon}", latitude, longitude);
        var iana = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
        logger.LogInformation("Converted coordinates to iana code, converting to utc");
        return TZConvert.GetTimeZoneInfo(iana).BaseUtcOffset.Hours;
    }

    public int? GetUtcOffset(string unknownTimezone)
    {
        logger.LogInformation("Trying to convert unknown timezone: {UnknownTimezone} to utc", unknownTimezone);
        if (IanaRegex().IsMatch(unknownTimezone))
        {
            logger.LogInformation("Timezone matches iana code pattern, converting");
            return TZConvert.GetTimeZoneInfo(unknownTimezone).BaseUtcOffset.Hours;
        }
        
        logger.LogInformation("Unable to identify timezone, returning same timezone: {UnknownTimezone}", unknownTimezone);
        return null;
    }
}