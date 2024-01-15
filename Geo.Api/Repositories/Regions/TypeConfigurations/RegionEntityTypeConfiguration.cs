namespace Geo.Api.Repositories.Regions.TypeConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal sealed class RegionEntityTypeConfiguration : IEntityTypeConfiguration<RegionEntity>
{
    public void Configure(EntityTypeBuilder<RegionEntity> builder)
    {
        builder.ToTable("regions");

        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CountryId)
            .HasColumnName("country_id")
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
            .IsRequired(false)
            .HasDefaultValue(null);

        builder.Property(e => e.Osm)
            .HasColumnName("osm")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(e => e.CountryId);
    }
}