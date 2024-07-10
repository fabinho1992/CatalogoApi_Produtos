using Infraestrutura.IdentityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Data
{
    public class ApiDbContextIdentity : IdentityDbContext<AppUser>
    {
        public ApiDbContextIdentity(DbContextOptions<ApiDbContextIdentity> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Refresh).HasMaxLength(256);
                entity.Property(e => e.RefreshTokenExpiryTime).IsRequired();
            });


            base.OnModelCreating(builder);
        }
    }
}
