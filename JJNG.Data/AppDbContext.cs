using System;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JJNG.Data
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<AppMenu> AppMenu { get; set; }
        public virtual DbSet<FncPayment> FncPayment { get; set; }
        public virtual DbSet<FncChannel> FncChannel { get; set; }
        public virtual DbSet<BrhFrontDeskAccounts> BrhFrontDeskAccounts { get; set; }
        public virtual DbSet<BrhFrontPaymentDetial> BrhFrontPaymentDetials { get; set; }
        public virtual DbSet<BrhConnectRecord> BrhConnectRecord { get; set; }

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
