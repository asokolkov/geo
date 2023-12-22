using System.Text.RegularExpressions;
using GeoTimeZone;
using Microsoft.Extensions.Logging;
using TimeZoneConverter;

namespace ObjectsLoader.Services.Impl;

public partial class TimezoneManager : ITimezoneManager
{
    private readonly ILogger<TimezoneManager> logger;
    [GeneratedRegex("[a-zA-Z-+_/\\d]*")] private static partial Regex IanaRegex();
    [GeneratedRegex("^(UTC[+-]\\d*).*")] private static partial Regex UtcRegex();

    public TimezoneManager(ILogger<TimezoneManager> logger)
    {
        this.logger = logger;
        logger.LogInformation("TimezoneManager initialized");
    }
    
    public string GetUtcTimezone(double latitude, double longitude)
    {
        logger.LogInformation("Searching utc timezone by latitude: {Lat} and longitude: {Lon}", latitude, longitude);
        var iana = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
        logger.LogInformation("Converted coordinates to iana code, converting to utc");
        return IanaToUtc(iana);
    }

    public string GetUtcTimezone(string unknownTimezone)
    {
        logger.LogInformation("Trying to convert unknown timezone: {UnknownTimezone} to utc", unknownTimezone);
        if (IanaRegex().IsMatch(unknownTimezone))
        {
            logger.LogInformation("Timezone matches iana code pattern, converting");
            return IanaToUtc(unknownTimezone);
        }
        
        if (UtcRegex().IsMatch(unknownTimezone))
        {
            logger.LogInformation("Timezone matches utc pattern, converting");
            var utc = unknownTimezone[^2] != '0' ? $"{unknownTimezone[..^1]}0{unknownTimezone[^1]}" : unknownTimezone;
            logger.LogInformation("Converted unknown timezone: {UnknownTimezone} to utc: {Utc}", unknownTimezone, utc);
            return utc;
        }
        
        logger.LogInformation("Unable to identify timezone, returning same timezone: {UnknownTimezone}", unknownTimezone);
        return unknownTimezone;
    }

    private string IanaToUtc(string iana)
    {
        logger.LogInformation("Converting iana code: {Iana} to utc", iana);
        var utc = TZConvert.GetTimeZoneInfo(iana);
        var negativeHours = utc.BaseUtcOffset.Hours < 0;
        var cleanHours = negativeHours ? utc.BaseUtcOffset.Hours * -1 : utc.BaseUtcOffset.Hours;
        var hoursOperator = negativeHours ? "-" : "+";
        var hoursString = cleanHours < 10 ? $"0{cleanHours}" : cleanHours.ToString();
        var result = $"UTC{hoursOperator}{hoursString}";
        logger.LogInformation("Converted iana code: {Iana} to utc: {Utc}", iana, result);
        return result;
    }
}