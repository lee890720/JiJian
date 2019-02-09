using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations.AppIdentityDb
{
    public partial class initialCreate22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AspNetUsers");
        }
    }
}
