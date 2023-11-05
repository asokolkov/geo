﻿namespace Geo.Api.V1.Countries.Controllers;

using Application.Countries.Commands.CreateCountryCommand;
using Application.Countries.Commands.UpdateCountryCommand;
using Application.Countries.Queries.GetCountryByIdQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;

[Route("api/v1/countries/")]
public sealed class V1CountriesController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public V1CountriesController(IMapper mapper, IMediator mediator)
    {
        this.mapper = mapper;
        this.mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(V1CountryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAsync(int id)
    {
        var request = new GetCountryByIdQuery(id);
        var country = await mediator.Send(request);
        var countryDto = mapper.Map<V1CountryDto>(country);
        return Ok(countryDto);
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