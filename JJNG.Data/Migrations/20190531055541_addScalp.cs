using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class addScalp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brh_Scalp",
                columns: table => new
                {
                    ScalpId = table.Column<long>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    Channel = table.Column<string>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Commission = table.Column<decimal>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    HouseNumber = table.Column<string>(nullable: false),
                    IsFinance = table.Column<bool>(nullable: false),
                    IsFinish = table.Column<bool>(nullable: false),
                    IsFront = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Settlement = table.Column<decimal>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_Scalp", x => x.ScalpId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brh_Scalp");
        }
    }
}
