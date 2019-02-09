using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class move : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMove",
                table: "Brh_ImprestRecord",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMove",
                table: "Brh_ImprestRecord");
        }
    }
}
