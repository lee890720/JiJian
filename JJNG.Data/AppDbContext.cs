using System;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using JJNG.Data.Personnel;
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
        public virtual DbSet<BrhStewardAccounts> BrhStewardAccounts { get; set; }
        public virtual DbSet<BrhStewardPaymentDetial> BrhStewardPaymentDetial { get; set; }
        public virtual DbSet<BrhImprestAccounts> BrhImprestAccounts { get; set; }
        public virtual DbSet<BrhConnectRecord> BrhConnectRecord { get; set; }
        public virtual DbSet<BrhEarningRecord> BrhEarningRecord { get; set; }
        public virtual DbSet<BrhExpendRecord> BrhExpendRecord { get; set; }
        public virtual DbSet<BrhImprestRecord> BrhImprestRecord { get; set; }
        public virtual DbSet<BrhMemo> BrhMemo { get; set; }
        public virtual DbSet<BrhClient> BrhClient { get; set; }
        public virtual DbSet<PsnNote> PsnNote { get; set; }
        public virtual DbSet<PsnAddress> PsnAddress { get; set; }
        public virtual DbSet<PsnNoteAccount> PsnNoteAccount { get; set; }
        public virtual DbSet<PsnAddressAccount> PsnAddressAccount { get; set; }

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
