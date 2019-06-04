using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class changeimprest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMove",
                table: "Brh_Scalp",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Purpose",
                table: "Brh_ImprestAccounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMove",
                table: "Brh_Scalp");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Brh_ImprestAccounts");
        }
    }
}
