using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class _1215change07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_Brh_ImprestRecord_ImprestAccountsId",
                table: "Brh_ImprestRecord",
                column: "ImprestAccountsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brh_ImprestRecord");

            migrationBuilder.DropTable(
                name: "Brh_ImprestAccounts");
        }
    }
}
