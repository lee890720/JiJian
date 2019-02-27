using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class fncMonthData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Average",
                table: "Fnc_MonthData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Fnc_MonthData",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValidAverage",
                table: "Fnc_MonthData",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Average",
                table: "Fnc_MonthData");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Fnc_MonthData");

            migrationBuilder.DropColumn(
                name: "ValidAverage",
                table: "Fnc_MonthData");
        }
    }
}
