using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityProj.Models.Entities;

namespace UniversityProj.Models.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Document");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name).HasMaxLength(40).IsRequired(true);
            builder.HasIndex(d => d.Name).IsUnique(true);

            builder.Property(d => d.DateOfExpiry).IsRequired(false).HasColumnType("date");

            builder.HasOne(d => d.TypeOfDoc).WithMany(t => t.Documents).HasForeignKey(d => d.TypeOfDocId).IsRequired(true);
        }
    }
}
