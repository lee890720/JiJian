using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class addcolor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Fnc_ChannelType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Brh_FrontDeskAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Fnc_ChannelType");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Brh_FrontDeskAccounts");
        }
    }
}
