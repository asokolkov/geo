namespace ObjectsLoader.Services;

public interface ITimezoneManager
{
    public string GetUtcTimezone(double latitude, double longitude);
    public string GetUtcTimezone(string unknownTimezone);
}