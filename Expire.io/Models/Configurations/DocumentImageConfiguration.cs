using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Expire.io.Models.Entities;

namespace Expire.io.Models.Configurations
{
    public class DocumentImageConfiguration : IEntityTypeConfiguration<DocumentImage>
    {
        public void Configure(EntityTypeBuilder<DocumentImage> builder)
        {
            builder.ToTable("DocumentImage");
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Document).WithOne(d => d.Image).HasForeignKey<DocumentImage>(d=>d.DocumentId).IsRequired();

            builder.Property(d => d.Image).IsRequired(true);
        }
    }
}
