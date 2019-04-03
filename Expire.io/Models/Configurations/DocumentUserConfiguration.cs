using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.Models.Entities;

namespace Expire.io.Models.Configurations
{
    public class DocumentUserConfiguration : IEntityTypeConfiguration<UserDocument>
    {
        public void Configure(EntityTypeBuilder<UserDocument> builder)
        {
            builder.ToTable("UserDocument");
            builder.HasKey(d => d.Id);

            builder.HasIndex(d => new { d.UserId, d.DocumentId }).IsUnique(true);

            builder.HasOne(d => d.User).WithMany(u => u.UserDocuments)
                .HasForeignKey(d => d.UserId).IsRequired();

            builder.HasOne(d => d.Document).WithMany(d => d.UserDocuments)
                .HasForeignKey(d => d.DocumentId).IsRequired();
        }
    }
}
