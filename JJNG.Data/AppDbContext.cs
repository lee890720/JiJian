using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JJNG.Data
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<AppMenu> AppMenu { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppMenu>(entity =>
            {
                entity.ToTable("App_Menu");
            });
        }
    }
}
