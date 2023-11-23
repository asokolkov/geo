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
        logger.LogInformation("{{method=\"timezone_manager_constructor\" status=\"success\" msg=\"Initialized\"}}");
    }
    
    public string GetUtcTimezone(double latitude, double longitude)
    {
        var iana = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
        logger.LogInformation("{{method=\"get_utc_timezone\" status=\"success\" msg=\"(lat: {Latitude}, lon: {Longitude}) -> {Iana}\"}}", latitude, longitude, iana);
        return IanaToUtc(iana);
    }

    public string GetUtcTimezone(string unknownTimezone)
    {
        if (IanaRegex().IsMatch(unknownTimezone))
        {
            return IanaToUtc(unknownTimezone);
        }
        
        if (UtcRegex().IsMatch(unknownTimezone))
        {
            var utc = unknownTimezone[^2] != '0' ? $"{unknownTimezone[..^1]}0{unknownTimezone[^1]}" : unknownTimezone;
            logger.LogInformation("{{method=\"get_utc_timezone\" status=\"success\" msg=\"{UnknownTimezone} -> {Utc}\"}}", unknownTimezone, utc);
            return utc;
        }
        
        logger.LogInformation("{{method=\"get_utc_timezone\" status=\"fail\" msg=\"Unknown timezone {UnknownTimezone}\"}}", unknownTimezone);
        return unknownTimezone;
    }

    private string IanaToUtc(string iana)
    {
        var utc = TZConvert.GetTimeZoneInfo(iana);
        var negativeHours = utc.BaseUtcOffset.Hours < 0;
        var cleanHours = negativeHours ? utc.BaseUtcOffset.Hours * -1 : utc.BaseUtcOffset.Hours;
        var hoursOperator = negativeHours ? "-" : "+";
        var hoursString = cleanHours < 10 ? $"0{cleanHours}" : cleanHours.ToString();
        var result = $"UTC{hoursOperator}{hoursString}";
        logger.LogInformation("{{method=\"iana_to_utc\" status=\"success\" msg=\"{Iana} -> {Utc}\"}}", iana, result);
        return result;
    }
}