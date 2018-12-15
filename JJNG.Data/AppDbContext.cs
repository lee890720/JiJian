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
        public virtual DbSet<FncPaymentType> FncPaymentType { get; set; }
        public virtual DbSet<FncChannelType> FncChannelType { get; set; }
        public virtual DbSet<FncExpendType> FncExpendType { get; set; }
        public virtual DbSet<FncEarningType> FncEarningType { get; set; }
        public virtual DbSet<BrhFrontDeskAccounts> BrhFrontDeskAccounts { get; set; }
        public virtual DbSet<BrhFrontPaymentDetial> BrhFrontPaymentDetials { get; set; }
        public virtual DbSet<BrhImprestAccounts> BrhImprestAccounts { get; set; }
        public virtual DbSet<BrhConnectRecord> BrhConnectRecord { get; set; }
        public virtual DbSet<BrhEarningRecord> BrhEarningRecord { get; set; }
        public virtual DbSet<BrhExpendRecord> BrhExpendRecord { get; set; }
        public virtual DbSet<BrhImprestRecord> BrhImprestRecord { get; set; }

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
