using AutoMapper;
using Geo.Api.Application.Countries.Commands.CreateCountryCommand;
using Geo.Api.Application.Countries.Commands.UpdateCountryCommand;
using Geo.Api.Application.Countries.Queries.GetCountryByIdQuery;
using Geo.Api.V1.Countries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Api.V1.Airports.Controllers;

[Route("api/v1/airports/")]
public sealed class V1AirportsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public V1AirportsController(IMapper mapper, IMediator mediator)
    {
        this.mapper = mapper;
        this.mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(V1AirportDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAsync(int id)
    {
        var request = new GetAirportByIdQuery(id);
        var airport = await mediator.Send(request);
        var airportDto = mapper.Map<V1AirportDto>(airport);
        return Ok(airportDto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(V1CountryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAsync([FromBody] V1CreateCountryRequestDto requestDto)
    {
        var request = new CreateCountryCommand(requestDto.Name, requestDto.Iso3116Alpha2Code,
            requestDto.Iso3166Alpha3Code, requestDto.PhoneCode, requestDto.PhoneMask, requestDto.Osm);
        var country = await mediator.Send(request);
        var countryDto = mapper.Map<V1CountryDto>(country);
        return Ok(countryDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(V1CountryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateAsync(int id, [FromBody] V1UpdateCountryRequestDto requestDto)
    {
        var request = new UpdateCountryCommand(id, requestDto.Name, requestDto.Iso3116Alpha2Code,
            requestDto.Iso3166Alpha3Code, requestDto.PhoneCode, requestDto.PhoneMask, requestDto.Osm);
        var country = await mediator.Send(request);
        var countryDto = mapper.Map<V1CountryDto>(country);
        return Ok(countryDto);
    }
}