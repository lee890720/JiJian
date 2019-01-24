﻿// <auto-generated />
using JJNG.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace JJNG.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190117043937_add-2")]
    partial class add2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JJNG.Data.AppMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<string>("Area");

                    b.Property<string>("Controller");

                    b.Property<string>("Description");

                    b.Property<int>("Follow");

                    b.Property<int>("Grade");

                    b.Property<string>("Ico");

                    b.Property<string>("Name");

                    b.Property<int>("Sequence");

                    b.Property<string>("State");

                    b.Property<string>("Url");

                    b.Property<bool>("Valid");

                    b.HasKey("Id");

                    b.ToTable("App_Menu");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhClient", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<string>("Follow");

                    b.Property<bool>("IsGood");

                    b.Property<bool>("IsSale");

                    b.Property<string>("Name");

                    b.Property<string>("Note");

                    b.HasKey("ClientId");

                    b.ToTable("Brh_Client");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhConnectRecord", b =>
                {
                    b.Property<int>("ConnectRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BillCount");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<int>("CardCount");

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<decimal>("HouseCash");

                    b.Property<string>("MorningStaff");

                    b.Property<string>("NigthStaff");

                    b.Property<string>("Note");

                    b.Property<decimal>("OtherCash");

                    b.HasKey("ConnectRecordId");

                    b.ToTable("Brh_ConnectRecord");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhEarningRecord", b =>
                {
                    b.Property<int>("EarningRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<string>("EarningType");

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<bool>("IsFinance");

                    b.Property<string>("Note");

                    b.Property<string>("PaymentType")
                        .IsRequired();

                    b.Property<string>("Source");

                    b.HasKey("EarningRecordId");

                    b.ToTable("Brh_EarningRecord");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhExpendRecord", b =>
                {
                    b.Property<int>("ExpendRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<string>("ConnectNumber");

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<string>("ExpendType");

                    b.Property<bool>("IsFinance");

                    b.Property<string>("Note");

                    b.Property<string>("PaymentType")
                        .IsRequired();

                    b.Property<string>("Purpose");

                    b.HasKey("ExpendRecordId");

                    b.ToTable("Brh_ExpendRecord");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhFrontDeskAccounts", b =>
                {
                    b.Property<long>("FrontDeskAccountsId");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<string>("Channel")
                        .IsRequired();

                    b.Property<string>("Color");

                    b.Property<int>("CustomerCount");

                    b.Property<string>("CustomerName")
                        .IsRequired();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<string>("FrontDeskLeader");

                    b.Property<string>("HouseNumber")
                        .IsRequired();

                    b.Property<bool>("IsFinance");

                    b.Property<bool>("IsFinish");

                    b.Property<bool>("IsFront");

                    b.Property<string>("Note");

                    b.Property<decimal>("Receivable");

                    b.Property<decimal>("Received");

                    b.Property<string>("RelationStaff");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Steward");

                    b.Property<string>("StewardLeader");

                    b.Property<decimal>("TotalPrice");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("FrontDeskAccountsId");

                    b.ToTable("Brh_FrontDeskAccounts");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhFrontPaymentDetial", b =>
                {
                    b.Property<int>("FrontPaymentDetialId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("FrontDeskAccountsId");

                    b.Property<decimal>("PayAmount");

                    b.Property<DateTime>("PayDate");

                    b.Property<string>("PayWay")
                        .IsRequired();

                    b.HasKey("FrontPaymentDetialId");

                    b.HasIndex("FrontDeskAccountsId");

                    b.ToTable("Brh_FrontPaymentDetial");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhFrontPaymentDetial2", b =>
                {
                    b.Property<int>("FrontPaymentDetialId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("FrontDeskAccountsId");

                    b.Property<decimal>("PayAmount");

                    b.Property<DateTime>("PayDate");

                    b.Property<string>("PayWay")
                        .IsRequired();

                    b.HasKey("FrontPaymentDetialId");

                    b.ToTable("Brh_FrontPaymentDetial2");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhImprestAccounts", b =>
                {
                    b.Property<int>("ImprestAccountsId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Balance");

                    b.Property<string>("BelongTo");

                    b.Property<string>("Department");

                    b.Property<decimal>("Equity");

                    b.Property<string>("ImprestAccountsName");

                    b.Property<string>("Manager");

                    b.HasKey("ImprestAccountsId");

                    b.ToTable("Brh_ImprestAccounts");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhImprestRecord", b =>
                {
                    b.Property<int>("ImprestRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<string>("ConnectNumber");

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<string>("ExpendType");

                    b.Property<int>("ImprestAccountsId");

                    b.Property<bool>("IsFinance");

                    b.Property<string>("Note");

                    b.Property<string>("PaymentType")
                        .IsRequired();

                    b.Property<string>("Purpose");

                    b.HasKey("ImprestRecordId");

                    b.HasIndex("ImprestAccountsId");

                    b.ToTable("Brh_ImprestRecord");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhMemo", b =>
                {
                    b.Property<int>("MemoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<bool>("IsFinish");

                    b.Property<string>("Memo");

                    b.Property<string>("Note");

                    b.HasKey("MemoId");

                    b.ToTable("Brh_Memo");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhStewardAccounts", b =>
                {
                    b.Property<long>("StewardAccountsId");

                    b.Property<decimal>("Amount");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<decimal>("Cost");

                    b.Property<string>("CustomerName")
                        .IsRequired();

                    b.Property<DateTime>("EnteringDate");

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<string>("FrontDesk");

                    b.Property<string>("FrontDeskLeader");

                    b.Property<string>("HouseNumber");

                    b.Property<bool>("IsFinance");

                    b.Property<bool>("IsFinish");

                    b.Property<bool>("IsSteward");

                    b.Property<string>("Note");

                    b.Property<string>("Product")
                        .IsRequired();

                    b.Property<string>("ProductType")
                        .IsRequired();

                    b.Property<decimal>("Profit");

                    b.Property<decimal>("Receivable");

                    b.Property<decimal>("Received");

                    b.Property<string>("RelationStaff");

                    b.Property<string>("StewardLeader");

                    b.HasKey("StewardAccountsId");

                    b.ToTable("Brh_StewardAccounts");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhStewardPaymentDetial", b =>
                {
                    b.Property<int>("StewardPaymentDetialId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("PayAmount");

                    b.Property<DateTime>("PayDate");

                    b.Property<string>("PayWay")
                        .IsRequired();

                    b.Property<long>("StewardAccountsId");

                    b.HasKey("StewardPaymentDetialId");

                    b.HasIndex("StewardAccountsId");

                    b.ToTable("Brh_StewardPaymentDetial");
                });

            modelBuilder.Entity("JJNG.Data.Finance.FncChannelType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChannelType");

                    b.Property<string>("Color");

                    b.Property<int>("Sequence");

                    b.HasKey("Id");

                    b.ToTable("Fnc_ChannelType");
                });

            modelBuilder.Entity("JJNG.Data.Finance.FncEarningType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EarningType");

                    b.Property<int>("Sequence");

                    b.HasKey("Id");

                    b.ToTable("Fnc_EarningType");
                });

            modelBuilder.Entity("JJNG.Data.Finance.FncExpendType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ExpendType");

                    b.Property<int>("Sequence");

                    b.HasKey("Id");

                    b.ToTable("Fnc_ExpendType");
                });

            modelBuilder.Entity("JJNG.Data.Finance.FncPaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PaymentType");

                    b.Property<int>("Sequence");

                    b.HasKey("Id");

                    b.ToTable("Fnc_PaymentType");
                });

            modelBuilder.Entity("JJNG.Data.Personnel.PsnAddress", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressAccountId");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("Note");

                    b.Property<string>("Phone");

                    b.Property<string>("Purpose");

                    b.HasKey("AddressId");

                    b.HasIndex("AddressAccountId");

                    b.ToTable("Psn_Address");
                });

            modelBuilder.Entity("JJNG.Data.Personnel.PsnAddressAccount", b =>
                {
                    b.Property<int>("AddressAccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName");

                    b.Property<string>("BelongTo");

                    b.Property<string>("Department");

                    b.Property<string>("Manager");

                    b.HasKey("AddressAccountId");

                    b.ToTable("Psn_AddressAccount");
                });

            modelBuilder.Entity("JJNG.Data.Personnel.PsnNote", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Account");

                    b.Property<string>("Branch")
                        .IsRequired();

                    b.Property<string>("EnteringStaff")
                        .IsRequired();

                    b.Property<string>("Note");

                    b.Property<int>("NoteAccountId");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<string>("Platform");

                    b.HasKey("NoteId");

                    b.HasIndex("NoteAccountId");

                    b.ToTable("Psn_Note");
                });

            modelBuilder.Entity("JJNG.Data.Personnel.PsnNoteAccount", b =>
                {
                    b.Property<int>("NoteAccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName");

                    b.Property<string>("BelongTo");

                    b.Property<string>("Department");

                    b.Property<string>("Manager");

                    b.HasKey("NoteAccountId");

                    b.ToTable("Psn_NoteAccount");
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhFrontPaymentDetial", b =>
                {
                    b.HasOne("JJNG.Data.Branch.BrhFrontDeskAccounts", "BrhFrontDeskAccounts")
                        .WithMany("BrhFrontPaymentDetial")
                        .HasForeignKey("FrontDeskAccountsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhImprestRecord", b =>
                {
                    b.HasOne("JJNG.Data.Branch.BrhImprestAccounts", "BrhImprestAccounts")
                        .WithMany("BrhImprestRecord")
                        .HasForeignKey("ImprestAccountsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("JJNG.Data.Branch.BrhStewardPaymentDetial", b =>
                {
                    b.HasOne("JJNG.Data.Branch.BrhStewardAccounts", "BrhStewardAccounts")
                        .WithMany("BrhStewardPaymentDetial")
                        .HasForeignKey("StewardAccountsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("JJNG.Data.Personnel.PsnAddress", b =>
                {
                    b.HasOne("JJNG.Data.Personnel.PsnAddressAccount", "PsnAddressAccount")
                        .WithMany("PsnAddress")
                        .HasForeignKey("AddressAccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("JJNG.Data.Personnel.PsnNote", b =>
                {
                    b.HasOne("JJNG.Data.Personnel.PsnNoteAccount", "PsnNoteAccount")
                        .WithMany("PsnNote")
                        .HasForeignKey("NoteAccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
