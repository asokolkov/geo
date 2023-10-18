namespace Geo.Api.V1.Countries.Controllers;

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public ActionResult GetAsync(int id)
    {
        return Ok();
    }
}