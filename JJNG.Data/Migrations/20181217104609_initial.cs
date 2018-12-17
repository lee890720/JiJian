using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "App_Menu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Follow = table.Column<int>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    Ico = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Valid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brh_ConnectRecord",
                columns: table => new
                {
                    ConnectRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillCount = table.Column<int>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    CardCount = table.Column<int>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    HouseCash = table.Column<double>(nullable: false),
                    MorningStaff = table.Column<string>(nullable: true),
                    NigthStaff = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    OtherCash = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_ConnectRecord", x => x.ConnectRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Brh_EarningRecord",
                columns: table => new
                {
                    EarningRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    EarningType = table.Column<string>(nullable: true),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    IsFinance = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    PaymentType = table.Column<string>(nullable: false),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_EarningRecord", x => x.EarningRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Brh_ExpendRecord",
                columns: table => new
                {
                    ExpendRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    ConnectNumber = table.Column<string>(nullable: true),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    ExpendType = table.Column<string>(nullable: true),
                    IsFinance = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    PaymentType = table.Column<string>(nullable: false),
                    Purpose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_ExpendRecord", x => x.ExpendRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Brh_FrontDeskAccounts",
                columns: table => new
                {
                    FrontDeskAccountsId = table.Column<long>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    Channel = table.Column<string>(nullable: false),
                    CustomerCount = table.Column<int>(nullable: false),
                    CustomerName = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    FrontDeskLeader = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<int>(nullable: false),
                    IsFinance = table.Column<bool>(nullable: false),
                    IsFinish = table.Column<bool>(nullable: false),
                    IsFront = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Receivable = table.Column<double>(nullable: false),
                    Received = table.Column<double>(nullable: false),
                    RelationStaff = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Steward = table.Column<string>(nullable: true),
                    StewardLeader = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_FrontDeskAccounts", x => x.FrontDeskAccountsId);
                });

            migrationBuilder.CreateTable(
                name: "Brh_ImprestAccounts",
                columns: table => new
                {
                    ImprestAccountsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Balance = table.Column<double>(nullable: false),
                    BelongTo = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Equity = table.Column<double>(nullable: false),
                    ImprestAccountsName = table.Column<string>(nullable: true),
                    Manager = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_ImprestAccounts", x => x.ImprestAccountsId);
                });

            migrationBuilder.CreateTable(
                name: "Brh_Memo",
                columns: table => new
                {
                    MemoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Branch = table.Column<string>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    IsFinish = table.Column<bool>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_Memo", x => x.MemoId);
                });

            migrationBuilder.CreateTable(
                name: "Fnc_ChannelType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelType = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_ChannelType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fnc_EarningType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EarningType = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_EarningType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fnc_ExpendType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExpendType = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_ExpendType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fnc_PaymentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentType = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brh_FrontPaymentDetial",
                columns: table => new
                {
                    FrontPaymentDetialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FrontDeskAccountsId = table.Column<long>(nullable: false),
                    PayAmount = table.Column<double>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: false),
                    PayWay = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_FrontPaymentDetial", x => x.FrontPaymentDetialId);
                    table.ForeignKey(
                        name: "FK_Brh_FrontPaymentDetial_Brh_FrontDeskAccounts_FrontDeskAccountsId",
                        column: x => x.FrontDeskAccountsId,
                        principalTable: "Brh_FrontDeskAccounts",
                        principalColumn: "FrontDeskAccountsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Brh_ImprestRecord",
                columns: table => new
                {
                    ImprestRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    ConnectNumber = table.Column<string>(nullable: true),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    ExpendType = table.Column<string>(nullable: true),
                    ImprestAccountsId = table.Column<int>(nullable: false),
                    IsFinance = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    PaymentType = table.Column<string>(nullable: false),
                    Purpose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_ImprestRecord", x => x.ImprestRecordId);
                    table.ForeignKey(
                        name: "FK_Brh_ImprestRecord_Brh_ImprestAccounts_ImprestAccountsId",
                        column: x => x.ImprestAccountsId,
                        principalTable: "Brh_ImprestAccounts",
                        principalColumn: "ImprestAccountsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brh_FrontPaymentDetial_FrontDeskAccountsId",
                table: "Brh_FrontPaymentDetial",
                column: "FrontDeskAccountsId");

            migrationBuilder.CreateIndex(
                name: "IX_Brh_ImprestRecord_ImprestAccountsId",
                table: "Brh_ImprestRecord",
                column: "ImprestAccountsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_Menu");

            migrationBuilder.DropTable(
                name: "Brh_ConnectRecord");

            migrationBuilder.DropTable(
                name: "Brh_EarningRecord");

            migrationBuilder.DropTable(
                name: "Brh_ExpendRecord");

            migrationBuilder.DropTable(
                name: "Brh_FrontPaymentDetial");

            migrationBuilder.DropTable(
                name: "Brh_ImprestRecord");

            migrationBuilder.DropTable(
                name: "Brh_Memo");

            migrationBuilder.DropTable(
                name: "Fnc_ChannelType");

            migrationBuilder.DropTable(
                name: "Fnc_EarningType");

            migrationBuilder.DropTable(
                name: "Fnc_ExpendType");

            migrationBuilder.DropTable(
                name: "Fnc_PaymentType");

            migrationBuilder.DropTable(
                name: "Brh_FrontDeskAccounts");

            migrationBuilder.DropTable(
                name: "Brh_ImprestAccounts");
        }
    }
}
