using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class _612change4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CooperationPrice",
                table: "Fnc_HouseType",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTABase",
                table: "Fnc_HouseType",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "OTAOrder1",
                table: "Fnc_HouseType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OTAOrder2",
                table: "Fnc_HouseType",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTAPre",
                table: "Fnc_HouseType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OTASpot",
                table: "Fnc_HouseType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StickerPrice",
                table: "Fnc_HouseType",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CooperationPrice",
                table: "Fnc_HouseType");

            migrationBuilder.DropColumn(
                name: "OTABase",
                table: "Fnc_HouseType");

            migrationBuilder.DropColumn(
                name: "OTAOrder1",
                table: "Fnc_HouseType");

            migrationBuilder.DropColumn(
                name: "OTAOrder2",
                table: "Fnc_HouseType");

            migrationBuilder.DropColumn(
                name: "OTAPre",
                table: "Fnc_HouseType");

            migrationBuilder.DropColumn(
                name: "OTASpot",
                table: "Fnc_HouseType");

            migrationBuilder.DropColumn(
                name: "StickerPrice",
                table: "Fnc_HouseType");
        }
    }
}
