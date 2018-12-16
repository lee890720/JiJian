using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class change121601 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinance",
                table: "Brh_EarningRecord",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinance",
                table: "Brh_EarningRecord");
        }
    }
}
