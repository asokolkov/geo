namespace Geo.Api.Application.Countries.Queries.GetCountryByIdQuery;

using Domain.Countries;
using JetBrains.Annotations;
using MediatR;
using Repositories.Countries;

[UsedImplicitly]
internal sealed class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Country?>
{
    private readonly ICountriesRepository repository;

    public GetCountryByIdQueryHandler(ICountriesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Country?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAsync(request.Id, cancellationToken);
    }
}