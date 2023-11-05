namespace Geo.Api.Application.Countries.Commands.UpdateCountryCommand;

using Domain.Countries;
using JetBrains.Annotations;
using MediatR;
using Repositories.Countries;

[UsedImplicitly]
internal sealed class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Country>
{
    private readonly ICountriesRepository repository;

    public UpdateCountryCommandHandler(ICountriesRepository repository)
    {
        this.repository = repository;
    }


    public async Task<Country> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        return await repository.UpdateAsync(request.Id, request.Name, request.Iso3116Alpha2Code,
            request.Iso3166Alpha3Code, request.PhoneCode, request.PhoneMask, request.Osm, cancellationToken);
    }
}