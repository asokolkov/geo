namespace ExternalTranslator.Models;

public class Translator
{
    public string Id { get; init; } = null!;
    public List<Restriction> Restrictions { get; init; } = new();
}