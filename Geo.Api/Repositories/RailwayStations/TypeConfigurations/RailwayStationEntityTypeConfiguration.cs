using Geo.Api.Repositories.RailwayStations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geo.Api.Repositories.RailwayStations.TypeConfigurations;

internal sealed class RailwayStationEntityTypeConfiguration : IEntityTypeConfiguration<RailwayStationEntity>
{
    public void Configure(EntityTypeBuilder<RailwayStationEntity> builder)
    {
        builder.ToTable("railway_stations");
        
        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.RzdCode)
            .HasColumnName("rzd_code")
            .IsRequired();

        builder.Property(e => e.IsMain)
            .HasColumnName("is_main")
            .IsRequired();

        builder.Property(e => e.CityId)
            .HasColumnName("city_id")
            .IsRequired();

        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

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

        builder.HasIndex(e => e.RzdCode).IsUnique();
        builder.HasIndex(e => e.CityId);
        builder.HasIndex(e => e.Osm).IsUnique();
    }
}