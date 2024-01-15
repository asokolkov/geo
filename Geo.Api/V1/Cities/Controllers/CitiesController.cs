using AutoMapper;
using Geo.Api.Repositories.Cities;
using Geo.Api.Repositories.Cities.Models;
using Geo.Api.V1.Cities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Api.V1.Cities.Controllers;

public sealed class CitiesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ICitiesRepository repository;

    public CitiesController(IMapper mapper, ICitiesRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpGet("city")]
    [ProducesResponseType(typeof(ResultDto<CityDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] int? id, [FromQuery] string? code,
        CancellationToken cancellationToken = default)
    {
        if (id.HasValue)
        {
            var cityEntity = await repository.GetAsync(id.Value, cancellationToken);
            if (cityEntity is null)
                return Ok(new ResultDto<CityDtoWithId>(StatusDto.Error));
            var dto = mapper.Map<CityDtoWithId>(cityEntity);
            return Ok(new ResultDto<CityDtoWithId>(StatusDto.Ok) { Result = dto });
        }

        if (code is not null)
        {
            var countryEntity = await repository.GetAsync(code, cancellationToken);
            if (countryEntity is null)
                return Ok(new ResultDto<CityDtoWithId>(StatusDto.Error));
            var dto = mapper.Map<CityDtoWithId>(countryEntity);
            return Ok(new ResultDto<CityDtoWithId>(StatusDto.Ok) { Result = dto });
        }

        return Ok(new ResultDto<CityDtoWithId>(StatusDto.Error));
    }

    [HttpPost("city")]
    [ProducesResponseType(typeof(ResultDto<CityDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CityDto cityDto,
        CancellationToken cancellationToken = default)
    {
        var countryEntity = await repository.CreateAsync(
            cityDto.LocationComponents.CountryId,
            cityDto.LocationComponents.RegionId,
            new CityNameEntity(cityDto.Name.Ru){En = cityDto.Name.En},
            new CityGeometryEntity{Lat = cityDto.Geometry.Lat, Lon = cityDto.Geometry.Lon},
            cityDto.Osm, cityDto.Code.En, cityDto.NeedToUpdate, cityDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<CityDtoWithId>(countryEntity);
        var result = new ResultDto<CityDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpPut("city/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<CityDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] CityDto cityDto,
        CancellationToken cancellationToken = default)
    {
        var countryEntity = await repository.UpdateAsync(id,
            cityDto.LocationComponents.CountryId,
            cityDto.LocationComponents.RegionId,
            new CityNameEntity(cityDto.Name.Ru){En = cityDto.Name.En},
            new CityGeometryEntity{Lat = cityDto.Geometry.Lat, Lon = cityDto.Geometry.Lon},
            cityDto.Osm, cityDto.Code.En, cityDto.NeedToUpdate, cityDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<CityDtoWithId>(countryEntity);
        var result = new ResultDto<CityDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpDelete("city/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<CityDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Ok(new ResultDto<CityDtoWithId>(StatusDto.Ok));
    }
}