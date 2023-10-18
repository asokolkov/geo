namespace Geo.Api.Repositories;

using Countries.Models;
using Microsoft.EntityFrameworkCore;

internal sealed class ApplicationContext : DbContext
{
    public DbSet<CountryEntity> Countries { get; set; } = null!;
}