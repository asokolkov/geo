namespace ExternalTranslator.Models;

public class Translator
{
    public string Id { get; init; } = null!;
    public DateTimeOffset? TimeCheckpoint { get; set; }
    public List<Restriction> Restrictions { get; init; } = new();
}