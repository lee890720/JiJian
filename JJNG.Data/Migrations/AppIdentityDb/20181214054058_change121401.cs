using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations.AppIdentityDb
{
    public partial class change121401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_BelongToDetial",
                columns: table => new
                {
                    BelongToDetialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BelongToId = table.Column<int>(nullable: false),
                    HouseName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_BelongToDetial", x => x.BelongToDetialId);
                    table.ForeignKey(
                        name: "FK_User_BelongToDetial_User_BelongTo_BelongToId",
                        column: x => x.BelongToId,
                        principalTable: "User_BelongTo",
                        principalColumn: "BelongToId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_BelongToDetial_BelongToId",
                table: "User_BelongToDetial",
                column: "BelongToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_BelongToDetial");
        }
    }
}
