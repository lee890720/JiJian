using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class addsteward : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brh_StewardAccounts",
                columns: table => new
                {
                    StewardAccountsId = table.Column<long>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    CustomerName = table.Column<string>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    HouseNumber = table.Column<int>(nullable: false),
                    IsFinance = table.Column<bool>(nullable: false),
                    IsFinish = table.Column<bool>(nullable: false),
                    IsFront = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: false),
                    ProductType = table.Column<string>(nullable: false),
                    Profit = table.Column<double>(nullable: false),
                    Receivable = table.Column<double>(nullable: false),
                    Received = table.Column<double>(nullable: false),
                    RelationStaff = table.Column<string>(nullable: true),
                    Steward = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_StewardAccounts", x => x.StewardAccountsId);
                });

            migrationBuilder.CreateTable(
                name: "Brh_StewardPaymentDetial",
                columns: table => new
                {
                    StewardPaymentDetialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayAmount = table.Column<double>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: false),
                    PayWay = table.Column<string>(nullable: false),
                    StewardAccountsId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_StewardPaymentDetial", x => x.StewardPaymentDetialId);
                    table.ForeignKey(
                        name: "FK_Brh_StewardPaymentDetial_Brh_StewardAccounts_StewardAccountsId",
                        column: x => x.StewardAccountsId,
                        principalTable: "Brh_StewardAccounts",
                        principalColumn: "StewardAccountsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brh_StewardPaymentDetial_StewardAccountsId",
                table: "Brh_StewardPaymentDetial",
                column: "StewardAccountsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brh_StewardPaymentDetial");

            migrationBuilder.DropTable(
                name: "Brh_StewardAccounts");
        }
    }
}
