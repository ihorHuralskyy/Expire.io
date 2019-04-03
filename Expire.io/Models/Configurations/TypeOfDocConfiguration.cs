using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Expire.io.Models.Entities;

namespace Expire.io.Models.Configurations
{
    public class TypeOfDocConfiguration : IEntityTypeConfiguration<TypeOfDoc>
    {
        public void Configure(EntityTypeBuilder<TypeOfDoc> builder)
        {
            builder.ToTable("TypeOfDoc");
            builder.HasKey(t => t.Id);

            builder.Property(n => n.Name).HasMaxLength(40).IsRequired();
            builder.HasIndex(t => t.Name).IsUnique(true);
        }
    }
}
