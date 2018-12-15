using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class _1215change06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FrontDeskLeader",
                table: "Brh_FrontDeskAccounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StewardLeader",
                table: "Brh_FrontDeskAccounts",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConnectNumber",
                table: "Brh_ExpendRecord",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "IsFinance",
                table: "Brh_ExpendRecord",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontDeskLeader",
                table: "Brh_FrontDeskAccounts");

            migrationBuilder.DropColumn(
                name: "StewardLeader",
                table: "Brh_FrontDeskAccounts");

            migrationBuilder.DropColumn(
                name: "IsFinance",
                table: "Brh_ExpendRecord");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectNumber",
                table: "Brh_ExpendRecord",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
