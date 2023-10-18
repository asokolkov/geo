namespace Geo.Api.Repositories.Countries.TypeConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal sealed class CountryEntityTypeConfiguration : IEntityTypeConfiguration<CountryEntity>
{
    public void Configure(EntityTypeBuilder<CountryEntity> builder)
    {
        builder.ToTable("countries");
        
        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Iso3116Alpha2Code)
            .HasColumnName("iso3116_alpha2")
            .HasMaxLength(2)
            .IsRequired();
        
        builder.Property(e => e.Iso3166Alpha3Code)
            .HasColumnName("iso3166_alpha3")
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(e => e.PhoneCode)
            .HasColumnName("phone_code")
            .HasMaxLength(4)
            .IsRequired();
        
        builder.Property(e => e.PhoneMask)
            .HasColumnName("phone_mask")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(e => e.Osm)
            .HasColumnName("osm")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");
        
        builder.HasIndex(e => e.Iso3116Alpha2Code).IsUnique();
        builder.HasIndex(e => e.Iso3166Alpha3Code).IsUnique();
    }
}