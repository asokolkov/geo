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
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(e => e.IataEn)
            .HasColumnName("iata_en")
            .HasMaxLength(3)
            .IsRequired();
        
        builder.Property(e => e.IataRu)
            .HasColumnName("iata_ru")
            .HasMaxLength(3)
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


        builder.HasOne(e => e.City)
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .IsRequired();

        builder.HasIndex(e => e.CityId);
        builder.HasIndex(e => e.IataEn).IsUnique();
        builder.HasIndex(e => e.IataRu).IsUnique();
        builder.HasIndex(e => e.Osm).IsUnique();
    }
}