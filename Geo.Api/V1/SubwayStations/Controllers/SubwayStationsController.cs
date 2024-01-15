using AutoMapper;
using Geo.Api.Repositories.SubwayStations;
using Geo.Api.Repositories.SubwayStations.Models;
using Geo.Api.V1.SubwayStations.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Api.V1.SubwayStations.Controllers;

public sealed class SubwayStationsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ISubwayStationsRepository repository;

    public SubwayStationsController(IMapper mapper, ISubwayStationsRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpGet("metro")]
    [ProducesResponseType(typeof(ResultDto<SubwayStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAsync([FromQuery] int? id, CancellationToken cancellationToken = default)
    {
        if (id.HasValue)
        {
            var airportEntity = await repository.GetAsync(id.Value, cancellationToken);
            var dto = mapper.Map<SubwayStationDtoWithId>(airportEntity);
            var result = new ResultDto<SubwayStationDtoWithId>(StatusDto.Ok) { Result = dto };
            return Ok(result);
        }

        var errorResult = new ResultDto<SubwayStationDtoWithId>(StatusDto.Error);
        return Ok(errorResult);
    }

    [HttpPost("metro")]
    [ProducesResponseType(typeof(ResultDto<SubwayStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] SubwayStationDto subwayStationDto,
        CancellationToken cancellationToken = default)
    {
        var airportEntity = await repository.CreateAsync(
            subwayStationDto.LocationComponents.CityId,
            new SubwayStationNameEntity(subwayStationDto.StationName.En) { Ru = subwayStationDto.StationName.Ru },
            new SubwayLineNameEntity(subwayStationDto.LineName.En) { Ru = subwayStationDto.LineName.Ru },
            new SubwayStationGeometryEntity
                { Lat = subwayStationDto.Geometry.Lat, Lon = subwayStationDto.Geometry.Lon },
            subwayStationDto.Osm, subwayStationDto.NeedToUpdate, cancellationToken);
        var dto = mapper.Map<SubwayStationDtoWithId>(airportEntity);
        var result = new ResultDto<SubwayStationDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpPut("metro/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<SubwayStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] SubwayStationDto subwayStationDto,
        CancellationToken cancellationToken = default)
    {
        var airportEntity = await repository.UpdateAsync(
            id, subwayStationDto.LocationComponents.CityId,
            new SubwayStationNameEntity(subwayStationDto.StationName.En) { Ru = subwayStationDto.StationName.Ru },
            new SubwayLineNameEntity(subwayStationDto.LineName.En) { Ru = subwayStationDto.LineName.Ru },
            new SubwayStationGeometryEntity
                { Lat = subwayStationDto.Geometry.Lat, Lon = subwayStationDto.Geometry.Lon },
            subwayStationDto.Osm, subwayStationDto.NeedToUpdate, cancellationToken);
        var dto = mapper.Map<SubwayStationDtoWithId>(airportEntity);
        var result = new ResultDto<SubwayStationDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpDelete("metro/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<SubwayStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Ok(new ResultDto<SubwayStationDtoWithId>(StatusDto.Ok));
    }
}