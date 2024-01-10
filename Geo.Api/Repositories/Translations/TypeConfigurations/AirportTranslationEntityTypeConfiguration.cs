using Geo.Api.Repositories.Translations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geo.Api.Repositories.Translations.TypeConfigurations;

internal sealed class AirportTranslationEntityTypeConfiguration : IEntityTypeConfiguration<TranslationEntity>
{
    public void Configure(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.ToTable("translations");
        
        builder.HasKey(e => new { e.EntityId, LanguageIso639 = e.LanguageId });

        builder.Property(e => e.EntityId)
            .HasColumnName("entity_id")
            .IsRequired();

        builder.Property(e => e.LanguageId)
            .HasColumnName("language_iso639")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(e => e.Translation)
            .HasColumnName("translation")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();
        
        builder.HasIndex(e => e.Translation);
    }
}