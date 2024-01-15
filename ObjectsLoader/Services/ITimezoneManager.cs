namespace ObjectsLoader.Services;

public interface ITimezoneManager
{
    public int? GetUtcOffset(double latitude, double longitude);
    public int? GetUtcOffset(string unknownTimezone);
}