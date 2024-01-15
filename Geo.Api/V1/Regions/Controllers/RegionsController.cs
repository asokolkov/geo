using AutoMapper;
using Geo.Api.Repositories.Regions;
using Geo.Api.Repositories.Regions.Models;
using Geo.Api.V1.Regions.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Api.V1.Regions.Controllers;

public sealed class RegionsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IRegionsRepository repository;

    public RegionsController(IMapper mapper, IRegionsRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    [HttpGet("region")]
    [ProducesResponseType(typeof(ResultDto<RegionDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] int? id, CancellationToken cancellationToken)
    {
        if (!id.HasValue)
            return Ok(new ResultDto<RegionDtoWithId>(StatusDto.Error));
        var regionEntity = await repository.GetAsync(id.Value, cancellationToken);
        if (regionEntity is null)
        {
            return Ok(new ResultDto<RegionDto>(StatusDto.Error));
        }

        var dto = mapper.Map<RegionDtoWithId>(regionEntity);
        var result = new ResultDto<RegionDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpPost("region")]
    [ProducesResponseType(typeof(ResultDto<RegionDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] RegionDto regionDto,
        CancellationToken cancellationToken = default)
    {
        var regionEntity = await repository.CreateAsync(
            regionDto.LocationComponents.CountryId,
            new RegionNameEntity(regionDto.Name.Ru) { En = regionDto.Name.En },
            regionDto.Osm, regionDto.NeedToUpdate,
            new RegionGeometryEntity { Lat = regionDto.Geometry?.Lat, Lon = regionDto.Geometry?.Lon },
            regionDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<RegionDtoWithId>(regionEntity);
        var result = new ResultDto<RegionDto>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }
    
    [HttpPut("region/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<RegionDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] RegionDto regionDto,
        CancellationToken cancellationToken = default)
    {
        var countryEntity = await repository.UpdateAsync(id,
            regionDto.LocationComponents.CountryId,
            new RegionNameEntity(regionDto.Name.Ru) { En = regionDto.Name.En },
            regionDto.Osm, regionDto.NeedToUpdate,
            new RegionGeometryEntity { Lat = regionDto.Geometry?.Lat, Lon = regionDto.Geometry?.Lon },
            regionDto.UtcOffset, cancellationToken);
        var dto = mapper.Map<RegionDtoWithId>(countryEntity);
        var result = new ResultDto<RegionDtoWithId>(StatusDto.Ok) { Result = dto };
        return Ok(result);
    }

    [HttpDelete("region/{id:int}")]
    [ProducesResponseType(typeof(ResultDto<RegionDtoWithId>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteAsync(id, cancellationToken);
        return Ok(new ResultDto<RegionDtoWithId>(StatusDto.Ok));
    }
}