using AutoMapper;
using Geo.Api.Repositories.Airports;
using Geo.Api.Repositories.Airports.Models;
using Geo.Api.V1.Airports.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Api.V1.Airports.Controllers;

public sealed class AirportsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAirportsRepository repository;

    public AirportsController(IMapper mapper, IAirportsRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpGet("airport")]
    [ProducesResponseType(typeof(ResultDto<AirportDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAsync([FromQuery] int? id, [FromQuery] string? code,
        CancellationToken cancellationToken = default)
    {
        if (id.HasValue)
        {
            var airportEntity = await repository.GetAsync(id.Value, cancellationToken);
            var dto = mapper.Map<AirportDtoWithId>(airportEntity);
            var result = new ResultDto<AirportDtoWithId>(StatusDto.Ok) { Result = dto };
            return Ok(result);
        }

        if (code is not null)
        {
            var airportEntity = await repository.GetAsync(code, cancellationToken);
            var dto = mapper.Map<AirportDtoWithId>(airportEntity);
            var result = new ResultDto<AirportDtoWithId>(StatusDto.Ok) { Result = dto };
            return Ok(result);
        }

        var errorResult = new ResultDto<AirportDtoWithId>(StatusDto.Error);
        return Ok(errorResult);
    }

    [HttpPost("airport")]
    [ProducesResponseType(typeof(ResultDto<AirportDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] AirportDto airportDto,
        CancellationToken cancellationToken = default)
    {
        var airportEntity = await repository.CreateAsync(
            airportDto.LocationComponents.CityId,
            new AirportNameEntity(airportDto.Name.Ru) { En = airportDto.Name.En },
            new AirportCodeEntity(airportDto.Code.En) { Ru = airportDto.Code.Ru },
            new AirportGeometryEntity { Lat = airportDto.Geometry.Lat, Lon = airportDto.Geometry.Lon },
            airportDto.Osm, airportDto.NeedToUpdate, airportDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<AirportDtoWithId>(airportEntity);
        var result = new ResultDto<AirportDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpPut("airport/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<AirportDtoWithId>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] AirportDto airportDto,
        CancellationToken cancellationToken = default)
    {
        var airportEntity = await repository.UpdateAsync(
            id, airportDto.LocationComponents.CityId,
            new AirportNameEntity(airportDto.Name.Ru) { En = airportDto.Name.En },
            new AirportCodeEntity(airportDto.Code.En) { Ru = airportDto.Code.Ru },
            new AirportGeometryEntity { Lat = airportDto.Geometry.Lat, Lon = airportDto.Geometry.Lon },
            airportDto.Osm, airportDto.NeedToUpdate, airportDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<AirportDtoWithId>(airportEntity);
        var result = new ResultDto<AirportDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }
    
    [HttpDelete("airport/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<AirportDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Ok(new ResultDto<AirportDtoWithId>(StatusDto.Ok));
    }
}