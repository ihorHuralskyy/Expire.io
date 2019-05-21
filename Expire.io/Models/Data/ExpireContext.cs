using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.Models.Configurations;
using Expire.io.Models.Entities;

namespace Expire.io.Models.Data
{
    public class ExpireContext: IdentityDbContext<User,Role,int>
    {
        public ExpireContext(DbContextOptions<ExpireContext> options):base(options) { }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentImage> DocumentImages { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }
        public DbSet<TypeOfDoc> TypeOfDocs { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<ManagerUser> ManagerUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new DocumentConfiguration());

            builder.ApplyConfiguration(new DocumentImageConfiguration());

            builder.ApplyConfiguration(new DocumentUserConfiguration());

            builder.ApplyConfiguration(new TypeOfDocConfiguration());

            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new UserImageConfiguration());

            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
