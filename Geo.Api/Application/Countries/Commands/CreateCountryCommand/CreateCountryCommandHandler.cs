namespace Geo.Api.Application.Countries.Commands.CreateCountryCommand;

using Domain.Countries;
using JetBrains.Annotations;
using MediatR;
using Repositories.Countries;

[UsedImplicitly]
internal sealed class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Country>
{
    private readonly ICountriesRepository repository;

    public CreateCountryCommandHandler(ICountriesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Country> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var country = await repository.CreateAsync(request.Name, request.Iso3116Alpha2Code, request.Iso3166Alpha3Code,
            request.PhoneCode, request.PhoneMask, request.Osm, cancellationToken);
        return country;
    }
}