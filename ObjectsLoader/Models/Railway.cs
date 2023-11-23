﻿namespace ObjectsLoader.Models;

public sealed class Railway
{
    public Guid Id { get; init; }
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public Guid RegionId { get; set; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public bool IsMain { get; init; }
    public string Rzd { get; init; } = null!;
    public int OsmId { get; init; }
    public string Timezone { get; init; } = null!;
    public string NameRu { get; init; } = null!;
    public int CityOsmId { get; init; }
}