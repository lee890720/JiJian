using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class addsteward2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Steward",
                table: "Brh_StewardAccounts",
                newName: "StewardLeader");

            migrationBuilder.AddColumn<string>(
                name: "FrontDesk",
                table: "Brh_StewardAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontDeskLeader",
                table: "Brh_StewardAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontDesk",
                table: "Brh_StewardAccounts");

            migrationBuilder.DropColumn(
                name: "FrontDeskLeader",
                table: "Brh_StewardAccounts");

            migrationBuilder.RenameColumn(
                name: "StewardLeader",
                table: "Brh_StewardAccounts",
                newName: "Steward");
        }
    }
}
