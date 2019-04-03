using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.Models.Entities;

namespace Expire.io.Models.Configurations
{
    public class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserImage> builder)
        {
            builder.ToTable("UserImage");
            builder.HasKey(u => u.Id);

            builder.HasOne(d => d.User).WithOne(u => u.Image).HasForeignKey<UserImage>(u=>u.UserId).IsRequired(true);
        }
    }
}
