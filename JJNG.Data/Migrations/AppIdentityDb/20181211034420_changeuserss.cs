using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations.AppIdentityDb
{
    public partial class changeuserss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Department_User_BelongTo_BelongToId",
                table: "User_Department");

            migrationBuilder.DropIndex(
                name: "IX_User_Department_BelongToId",
                table: "User_Department");

            migrationBuilder.DropColumn(
                name: "BelongToId",
                table: "User_Department");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BelongToId",
                table: "User_Department",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_Department_BelongToId",
                table: "User_Department",
                column: "BelongToId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Department_User_BelongTo_BelongToId",
                table: "User_Department",
                column: "BelongToId",
                principalTable: "User_BelongTo",
                principalColumn: "BelongToId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
