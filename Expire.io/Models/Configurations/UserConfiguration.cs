using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.Models.Entities;

namespace Expire.io.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName).HasColumnName("FName").HasMaxLength(30).IsRequired();
            builder.Property(u => u.LastName).HasColumnName("LName").HasMaxLength(30).IsRequired();
        }
    }
}
