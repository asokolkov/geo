namespace ExternalTranslator.Models;

public class Translator
{
    public string Id { get; init; } = null!;
    public string Url { get; init; } = null!;
    public int CharsMax { get; init; }
    public TimeSpan CharsPeriod { get; init; }
    public int QueriesMax { get; init; }
    public TimeSpan QueriesPeriod { get; init; }
}