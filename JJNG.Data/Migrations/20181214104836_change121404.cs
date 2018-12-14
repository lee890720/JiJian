using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class change121404 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MorningStaff",
                table: "Brh_ConnectRecord",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NigthStaff",
                table: "Brh_ConnectRecord",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MorningStaff",
                table: "Brh_ConnectRecord");

            migrationBuilder.DropColumn(
                name: "NigthStaff",
                table: "Brh_ConnectRecord");
        }
    }
}
