using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations.AppIdentityDb
{
    public partial class changeposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Position_User_Department_DepartmentId",
                table: "User_Position");

            migrationBuilder.DropIndex(
                name: "IX_User_Position_DepartmentId",
                table: "User_Position");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "User_Position");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "User_Position",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_Position_DepartmentId",
                table: "User_Position",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Position_User_Department_DepartmentId",
                table: "User_Position",
                column: "DepartmentId",
                principalTable: "User_Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
