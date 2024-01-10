using AutoMapper;
using Geo.Api.Application.Airports.Commands.CreateAirportCommand;
using Geo.Api.Application.Airports.Queries.GetAirportByIataQuery;
using Geo.Api.Application.Airports.Queries.GetAirportByIdQuery;
using Geo.Api.Application.Airports.Queries.SearchAirportsQuery;
using Geo.Api.V1.Airports.Models;
using Geo.Api.V1.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Api.V1.Airports.Controllers;

[Route("api")]
public sealed class V1AirportsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public V1AirportsController(IMapper mapper, IMediator mediator)
    {
        this.mapper = mapper;
        this.mediator = mediator;
    }

    [HttpGet("airport")]
    [ProducesResponseType(typeof(GetResultDto<AirportDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAsync([FromQuery] GetAirportDto getAirportDto)
    {
        var request = new GetAirportQuery { Id = getAirportDto.Id, Code = getAirportDto.Code };
        var airport = await mediator.Send(request);
        var airportDto = mapper.Map<AirportDto>(airport);
        return Ok(airportDto);
    }

    [HttpGet("{lang}/search_objects/avia")]
    [ProducesResponseType(typeof(SearchResultDto<SearchResultElementDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> SearchAsync([FromQuery] SearchAirportsDto searchDto)
    {
        var request = new SearchAirportsAndCitiesQuery();
        var airports = await mediator.Send(request);
        var airportsDto = mapper.Map<SearchResultDto>(airports);
        return Ok(airportsDto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] CreateAirportDtp requestDto)
    {
        var request = new CreateAirportCommand(requestDto.Name, requestDto.Iso3116Alpha2Code,
            requestDto.Iso3166Alpha3Code, requestDto.PhoneCode, requestDto.PhoneMask, requestDto.Osm);
        var country = await mediator.Send(request);
        var countryDto = mapper.Map<AirportDto>(country);
        return Ok(countryDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateAirportRequestDto requestDto)
    {
        var request = new UpdateAirportCommand(id, requestDto.Name, requestDto.Iso3116Alpha2Code,
            requestDto.Iso3166Alpha3Code, requestDto.PhoneCode, requestDto.PhoneMask, requestDto.Osm);
        var country = await mediator.Send(request);
        var countryDto = mapper.Map<AirportDto>(country);
        return Ok(countryDto);
    }
}