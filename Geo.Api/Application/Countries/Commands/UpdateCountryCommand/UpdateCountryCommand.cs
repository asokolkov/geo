namespace Geo.Api.Application.Countries.Commands.UpdateCountryCommand;

using Domain.Countries;
using MediatR;

internal sealed record UpdateCountryCommand(int Id, string Name, string Iso3116Alpha2Code, string Iso3166Alpha3Code,
    string PhoneCode, string PhoneMask, string Osm) : IRequest<Country>;