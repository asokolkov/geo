using Geo.Api.Repositories.Cities.Models;
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
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Iata)
            .HasColumnName("iata")
            .HasMaxLength(3)
            .HasDefaultValue(null)
            .IsRequired(false);
        
        builder.Property(e => e.Latitude)
            .HasColumnName("latitude")
            .IsRequired();

        builder.Property(e => e.Longitude)
            .HasColumnName("longitude")
            .IsRequired();
        
        builder.Property(e => e.Timezone)
            .HasColumnName("timezone")
            .HasMaxLength(6)
            .IsRequired();
        
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
            .IsRequired();

        builder.HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .IsRequired();

        builder.HasOne(e => e.Region)
            .WithMany()
            .HasForeignKey(e => e.RegionId)
            .IsRequired(false);
        
        builder.HasMany(e => e.Translations)
            .WithOne()
            .HasForeignKey(e => new {Type = "city", e.EntityId})
            .IsRequired(false);
        
        builder.HasIndex(e => e.Iata).IsUnique();
        builder.HasIndex(e => e.Osm).IsUnique();
        builder.HasIndex(e => e.RegionId);
        builder.HasIndex(e => e.CountryId);
    }
}