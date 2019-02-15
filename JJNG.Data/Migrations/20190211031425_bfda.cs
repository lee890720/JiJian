using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class bfda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Brh_FrontDeskAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Brh_FrontDeskAccounts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Brh_FrontDeskAccounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Brh_FrontDeskAccounts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Brh_FrontDeskAccounts");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Brh_FrontDeskAccounts");
        }
    }
}
