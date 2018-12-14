using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    HouseNumber = table.Column<int>(nullable: false),
                    IsFinance = table.Column<bool>(nullable: false),
                    IsFinish = table.Column<bool>(nullable: false),
                    IsFront = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Receivable = table.Column<double>(nullable: false),
                    Received = table.Column<double>(nullable: false),
                    RelationStaff = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_FrontDeskAccounts", x => x.FrontDeskAccountsId);
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

            migrationBuilder.CreateIndex(
                name: "IX_Brh_FrontPaymentDetial_FrontDeskAccountsId",
                table: "Brh_FrontPaymentDetial",
                column: "FrontDeskAccountsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brh_FrontPaymentDetial");

            migrationBuilder.DropTable(
                name: "Brh_FrontDeskAccounts");
        }
    }
}
