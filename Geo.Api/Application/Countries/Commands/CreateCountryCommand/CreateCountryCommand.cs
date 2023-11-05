namespace Geo.Api.Application.Countries.Commands.CreateCountryCommand;

using Domain.Countries;
using MediatR;

internal sealed record CreateCountryCommand(string Name, string Iso3116Alpha2Code, string Iso3166Alpha3Code,
    string PhoneCode, string PhoneMask, string Osm) : IRequest<Country>;