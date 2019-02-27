using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class fncMonthData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fnc_MonthData",
                columns: table => new
                {
                    MonthDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<int>(nullable: false),
                    Earning = table.Column<decimal>(nullable: false),
                    Expend = table.Column<decimal>(nullable: false),
                    HouseAmount = table.Column<decimal>(nullable: false),
                    HouseCount = table.Column<int>(nullable: false),
                    HouseTotal = table.Column<int>(nullable: false),
                    Month = table.Column<DateTime>(nullable: false),
                    SaleAmount = table.Column<decimal>(nullable: false),
                    SaleProfit = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_MonthData", x => x.MonthDataId);
                    table.ForeignKey(
                        name: "FK_Fnc_MonthData_Fnc_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Fnc_Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fnc_MonthData_BranchId",
                table: "Fnc_MonthData",
                column: "BranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fnc_MonthData");
        }
    }
}
