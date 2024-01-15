using Geo.Api.Repositories.Cities.Models;
using Geo.Api.Repositories.Countries.Models;
using Geo.Api.Repositories.Regions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geo.Api.Repositories.Cities.TypeConfigurations;

internal sealed class CityEntityTypeConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder.ToTable("cities");
        
        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(e => e.Code)
            .HasColumnName("iata")
            .HasMaxLength(3)
            .IsRequired();
        
        builder.Property(e => e.Geometry)
            .HasColumnName("geometry")
            .HasColumnType("jsonb")
            .IsRequired();
        
        builder.Property(e => e.UtcOffset)
            .HasColumnName("utc_offset")
            .IsRequired(false);

        builder.Property(e => e.NeedToUpdate)
            .HasColumnName("need_to_update")
            .IsRequired();
        
        builder.Property(e => e.UtcOffset)
            .HasColumnName("utc_offset")
            .IsRequired(false);
        
        builder.Property(e => e.Osm)
            .HasColumnName("osm")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at")
            .HasDefaultValue(null)
            .IsRequired(false);

        builder.Property(e => e.CountryId)
            .IsRequired();

        builder.Property(e => e.RegionId)
            .IsRequired(false);

        builder.HasOne<CountryEntity>()
            .WithMany()
            .HasForeignKey(e => e.CountryId);
        
        builder.HasOne<RegionEntity>()
            .WithMany()
            .HasForeignKey(e => e.RegionId);

        builder.HasIndex(e => e.Code).IsUnique();
        builder.HasIndex(e => e.Osm).IsUnique();
        builder.HasIndex(e => e.RegionId);
        builder.HasIndex(e => e.CountryId);
    }
}