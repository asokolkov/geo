using Geo.Api.Repositories.Translations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geo.Api.Repositories.Translations.TypeConfigurations;

internal sealed class TranslationEntityTypeConfiguration : IEntityTypeConfiguration<TranslationEntity>
{
    public void Configure(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.ToTable("translations");
        
        builder.HasKey(e => new { e.Type, e.EntityId, e.LanguageId });

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.EntityId)
            .HasColumnName("entity_id")
            .IsRequired();

        builder.Property(e => e.LanguageId)
            .HasColumnName("language_iso639")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName("translation")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();
        
        builder.HasIndex(e => e.Value);
        
    }
}