using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace JJNG.Data.AppIdentity
{
    public class AppIdentityDbContext : IdentityDbContext<AppIdentityUser>
    {
        public virtual DbSet<UserBelongTo> UserBelongTo { get; set; }
        public virtual DbSet<UserDepartment> UserDepartment { get; set; }
        public virtual DbSet<UserPosition> UserPosition { get; set; }
        public virtual DbSet<AppIdentityRole> AppIdentityRole { get; set; }
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
