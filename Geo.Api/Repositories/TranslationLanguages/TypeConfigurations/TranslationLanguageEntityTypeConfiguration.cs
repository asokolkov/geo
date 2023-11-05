using Geo.Api.Repositories.TranslationLanguages.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geo.Api.Repositories.TranslationLanguages.TypeConfigurations;

internal sealed class TranslationLanguageEntityTypeConfiguration : IEntityTypeConfiguration<TranslationLanguageEntity>
{
    public void Configure(EntityTypeBuilder<TranslationLanguageEntity> builder)
    {
        builder.ToTable("translation_languages");
        
        builder.HasKey(e => e.Language);

        builder.Property(e => e.Language)
            .HasColumnName("language")
            .HasMaxLength(2)
            .IsRequired();
        
        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(255)
            .IsRequired();
    }
}