using System.Text.RegularExpressions;
using GeoTimeZone;
using TimeZoneConverter;

namespace ObjectsLoader.Services.Impl;

public class TimezoneManager : ITimezoneManager
{
    public string GetUtcTimezone(double latitude, double longitude)
    {
        var iana = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
        return IanaToUtc(iana);
    }

    public string GetUtcTimezone(string unknownTimezone)
    {
        if (Regex.IsMatch(unknownTimezone, @"[a-zA-Z-+_/\d]*"))
        {
            return IanaToUtc(unknownTimezone);
        }
        
        var utcMatch = Regex.Match(unknownTimezone, @"^(UTC[+-]\d*).*");
        if (!utcMatch.Success)
        {
            return unknownTimezone;
        }
        
        var utc = utcMatch.Groups[1].Value;
        return utc[^2] != '0' ? $"{utc[..^1]}0{utc[^1]}" : utc;
    }

    private string IanaToUtc(string iana)
    {
        var utc = TZConvert.GetTimeZoneInfo(iana);
        var negativeHours = utc.BaseUtcOffset.Hours < 0;
        var cleanHours = negativeHours ? utc.BaseUtcOffset.Hours * -1 : utc.BaseUtcOffset.Hours;
        var hoursOperator = negativeHours ? "-" : "+";
        var hoursString = cleanHours < 10 ? $"0{cleanHours}" : cleanHours.ToString();
        return $"UTC{hoursOperator}{hoursString}";
    }
}