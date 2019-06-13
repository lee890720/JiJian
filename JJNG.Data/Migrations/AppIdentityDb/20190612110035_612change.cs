using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations.AppIdentityDb
{
    public partial class _612change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_BranchDetial");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_BranchDetial",
                columns: table => new
                {
                    BranchDetialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<int>(nullable: false),
                    HouseNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_BranchDetial", x => x.BranchDetialId);
                    table.ForeignKey(
                        name: "FK_User_BranchDetial_User_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "User_Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_BranchDetial_BranchId",
                table: "User_BranchDetial",
                column: "BranchId");
        }
    }
}
