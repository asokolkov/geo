using ExternalTranslator.Enums;

namespace ExternalTranslator.Models;

public class Restriction
{
    public RestrictionType Type { get; init; }
    public int MaxAmount { get; init; }
    public TimeSpan Period { get; init; }
    public int CurrentAmount { get; set; }
    public bool LimitReached(int amount) => CurrentAmount + amount >= MaxAmount;
}