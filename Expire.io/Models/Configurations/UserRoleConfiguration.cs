using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityProj.Models.Entities;

namespace UniversityProj.Models.Configurations
{
    public class UserRoleConfiguration: IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasIndex(r => new { r.RoleId, r.UserId }).IsUnique();

            builder.HasOne(r => r.User).WithMany(u => u.UserRoles)
                .HasForeignKey(r => r.UserId).IsRequired();

            builder.HasOne(r => r.Role).WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId).IsRequired();
        }

         
    }
}
