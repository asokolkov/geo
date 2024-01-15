using Geo.Api.Repositories.SubwayStations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geo.Api.Repositories.SubwayStations.TypeConfiguration;

internal sealed class SubwayStationEntityTypeConfiguration : IEntityTypeConfiguration<SubwayStationEntity>
{
    public void Configure(EntityTypeBuilder<SubwayStationEntity> builder)
    {
        builder.ToTable("subway_stations");
        
        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CityId)
            .IsRequired();

        builder.Property(e => e.StationName)
            .HasColumnName("station_name")
            .HasColumnType("jsonb")
            .IsRequired();
        
        builder.Property(e => e.LineName)
            .HasColumnName("line_name")
            .HasColumnType("jsonb")
            .IsRequired();
        
        builder.Property(e => e.Geometry)
            .HasColumnName("geometry")
            .HasColumnType("jsonb")
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

        builder.Property(e => e.NeedToUpdate)
            .HasColumnName("need_automatic_update")
            .HasDefaultValue(true)
            .IsRequired();


        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .IsRequired();
        
        builder.HasIndex(e => e.CityId);
        builder.HasIndex(e => e.Osm).IsUnique();
    }
}