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

        builder.Property(e => e.Code)
            .HasColumnName("code")
            .HasColumnType("jsonb")
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
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(e => e.Geometry)
            .HasColumnName("geometry")
            .HasColumnType("jsonb")
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

        builder.Property(e => e.NeedToUpdate)
            .HasColumnName("need_to_update")
            .IsRequired();

        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .IsRequired();
        
        builder.HasIndex(e => e.CityId);
        builder.HasIndex(e => e.Osm).IsUnique();
    }
}