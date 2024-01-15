using Geo.Api.Repositories.Airports.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geo.Api.Repositories.Airports.TypeConfiguration;

internal sealed class AirportEntityTypeConfiguration : IEntityTypeConfiguration<AirportEntity>
{
    public void Configure(EntityTypeBuilder<AirportEntity> builder)
    {
        builder.ToTable("airports");
        
        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CityId)
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasColumnType("jsonb")
            .IsRequired();
        
        builder.Property(e => e.Code)
            .HasColumnName("code")
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

        builder.Property(e => e.NeedAutomaticUpdate)
            .HasColumnName("need_automatic_update")
            .IsRequired();


        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .IsRequired();
        
        builder.HasIndex(e => e.CityId);
        builder.HasIndex(e => e.Osm).IsUnique();
    }
}