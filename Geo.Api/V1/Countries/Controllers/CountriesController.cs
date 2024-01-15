using Geo.Api.Repositories.Countries;
using Geo.Api.Repositories.Countries.Models;
using Geo.Api.V1.Countries.Models;

namespace Geo.Api.V1.Countries.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public sealed class CountriesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ICountriesRepository repository;

    public CountriesController(IMapper mapper, ICountriesRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpGet("country")]
    [ProducesResponseType(typeof(ResultDto<CountryDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] int? id, [FromQuery] string? code,
        CancellationToken cancellationToken = default)
    {
        if (id.HasValue)
        {
            var countryEntity = await repository.GetAsync(id.Value, cancellationToken);
            if (countryEntity is null)
                return Ok(new ResultDto<CountryDtoWithId>(StatusDto.Error));
            var dto = mapper.Map<CountryDtoWithId>(countryEntity);
            return Ok(new ResultDto<CountryDtoWithId>(StatusDto.Ok) { Result = dto });
        }

        if (code is not null)
        {
            var countryEntity = await repository.GetAsync(code, cancellationToken);
            if (countryEntity is null)
                return Ok(new ResultDto<CountryDtoWithId>(StatusDto.Error));
            var dto = mapper.Map<CountryDtoWithId>(countryEntity);
            return Ok(new ResultDto<CountryDtoWithId>(StatusDto.Ok) { Result = dto });
        }

        return Ok(new ResultDto<CountryDtoWithId>(StatusDto.Error));
    }

    [HttpPost("country")]
    [ProducesResponseType(typeof(ResultDto<CountryDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] CountryDto requestDto,
        CancellationToken cancellationToken = default)
    {
        var countryEntity = await repository.CreateAsync(
            new CountryNameEntity(requestDto.Name.Ru) { En = requestDto.Name.En },
            requestDto.Code.Iso3116Alpha2, requestDto.Code.Iso3166Alpha3, requestDto.Phone.Code,
            requestDto.Phone.Mask, requestDto.Osm, requestDto.NeedToUpdate,
            new CountryGeometryEntity { Lat = requestDto.Geometry?.Lat, Lon = requestDto.Geometry?.Lon },
            cancellationToken);
        var dto = mapper.Map<CountryDtoWithId>(countryEntity);
        var result = new ResultDto<CountryDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpPut("country/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<CountryDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] CountryDto requestDto,
        CancellationToken cancellationToken = default)
    {
        var countryEntity = await repository.UpdateAsync(id,
            new CountryNameEntity(requestDto.Name.Ru) { En = requestDto.Name.En },
            requestDto.Code.Iso3116Alpha2, requestDto.Code.Iso3166Alpha3, requestDto.Phone.Code,
            requestDto.Phone.Mask, requestDto.Osm, requestDto.NeedToUpdate,
            new CountryGeometryEntity { Lat = requestDto.Geometry?.Lat, Lon = requestDto.Geometry?.Lon },
            cancellationToken);
        var dto = mapper.Map<CountryDtoWithId>(countryEntity);
        var result = new ResultDto<CountryDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpDelete("country/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<CountryDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Ok(new ResultDto<CountryDtoWithId>(StatusDto.Ok));
    }
}