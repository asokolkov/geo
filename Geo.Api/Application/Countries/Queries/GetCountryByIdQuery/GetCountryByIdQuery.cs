namespace Geo.Api.Application.Countries.Queries.GetCountryByIdQuery;

using Domain.Countries;
using MediatR;

internal sealed record GetCountryByIdQuery(int Id) : IRequest<Country?>;