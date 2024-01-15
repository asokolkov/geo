using AutoMapper;
using Geo.Api.Repositories.RailwayStations;
using Geo.Api.Repositories.RailwayStations.Models;
using Geo.Api.V1.RailStations.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Api.V1.RailStations.Controllers;

public sealed class RailStationsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IRailwayStationsRepository repository;

    public RailStationsController(IMapper mapper, IRailwayStationsRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpGet("rail_station")]
    [ProducesResponseType(typeof(ResultDto<RailStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAsync([FromQuery] int? id,
        CancellationToken cancellationToken = default)
    {
        if (id.HasValue)
        {
            var railwayStationEntity = await repository.GetAsync(id.Value, cancellationToken);
            var dto = mapper.Map<RailStationDtoWithId>(railwayStationEntity);
            var result = new ResultDto<RailStationDtoWithId>(StatusDto.Ok) { Result = dto };
            return Ok(result);
        }

        var errorResult = new ResultDto<RailStationDtoWithId>(StatusDto.Error);
        return Ok(errorResult);
    }

    [HttpPost("rail_station")]
    [ProducesResponseType(typeof(ResultDto<RailStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] RailStationDto railStationDto,
        CancellationToken cancellationToken = default)
    {
        var railwayStationEntity = await repository.CreateAsync(
            railStationDto.LocationComponents.CityId,
            new RailwayStationCodeEntity(railStationDto.Code.Express3),
            new RailwayStationNameEntity(railStationDto.Name.En) { Ru = railStationDto.Name.Ru },
            new RailwayStationGeometryEntity { Lat = railStationDto.Geometry.Lat, Lon = railStationDto.Geometry.Lon },
            railStationDto.Osm, railStationDto.NeedToUpdate, railStationDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<RailStationDtoWithId>(railwayStationEntity);
        var result = new ResultDto<RailStationDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpPut("rail_station/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<RailStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] RailStationDto railStationDto,
        CancellationToken cancellationToken = default)
    {
        var railwayStationEntity = await repository.UpdateAsync(
            id, railStationDto.LocationComponents.CityId,
            new RailwayStationCodeEntity(railStationDto.Code.Express3),
            new RailwayStationNameEntity(railStationDto.Name.En) { Ru = railStationDto.Name.Ru },
            new RailwayStationGeometryEntity { Lat = railStationDto.Geometry.Lat, Lon = railStationDto.Geometry.Lon },
            railStationDto.Osm, railStationDto.NeedToUpdate, railStationDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<RailStationDtoWithId>(railwayStationEntity);
        var result = new ResultDto<RailStationDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpDelete("rail_station/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<RailStationDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Ok(new ResultDto<RailStationDtoWithId>(StatusDto.Ok));
    }
}