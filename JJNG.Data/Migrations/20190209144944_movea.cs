using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class movea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MoveAmount",
                table: "Brh_ImprestAccounts",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoveAmount",
                table: "Brh_ImprestAccounts");
        }
    }
}
