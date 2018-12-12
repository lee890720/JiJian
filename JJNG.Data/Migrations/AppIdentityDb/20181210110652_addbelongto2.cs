using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations.AppIdentityDb
{
    public partial class addbelongto2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_BelongTo",
                columns: table => new
                {
                    BelongToId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BelongToName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_BelongTo", x => x.BelongToId);
                });

            migrationBuilder.CreateTable(
                name: "User_Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BelongToId = table.Column<int>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Department", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_User_Department_User_BelongTo_BelongToId",
                        column: x => x.BelongToId,
                        principalTable: "User_BelongTo",
                        principalColumn: "BelongToId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Position",
                columns: table => new
                {
                    PositionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(nullable: false),
                    PositionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Position", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_User_Position_User_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "User_Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Department_BelongToId",
                table: "User_Department",
                column: "BelongToId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Position_DepartmentId",
                table: "User_Position",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Position");

            migrationBuilder.DropTable(
                name: "User_Department");

            migrationBuilder.DropTable(
                name: "User_BelongTo");
        }
    }
}
