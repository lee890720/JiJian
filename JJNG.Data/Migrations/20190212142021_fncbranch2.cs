using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class fncbranch2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fnc_Branch",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_Branch", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Fnc_HouseType",
                columns: table => new
                {
                    HouseTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<int>(nullable: false),
                    HouseType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_HouseType", x => x.HouseTypeId);
                    table.ForeignKey(
                        name: "FK_Fnc_HouseType_Fnc_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Fnc_Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fnc_HouseNumber",
                columns: table => new
                {
                    HouseNumberId = table.Column<string>(nullable: false),
                    HouseNumber = table.Column<string>(nullable: true),
                    HouseTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_HouseNumber", x => x.HouseNumberId);
                    table.ForeignKey(
                        name: "FK_Fnc_HouseNumber_Fnc_HouseType_HouseTypeId",
                        column: x => x.HouseTypeId,
                        principalTable: "Fnc_HouseType",
                        principalColumn: "HouseTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fnc_HouseNumber_HouseTypeId",
                table: "Fnc_HouseNumber",
                column: "HouseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fnc_HouseType_BranchId",
                table: "Fnc_HouseType",
                column: "BranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fnc_HouseNumber");

            migrationBuilder.DropTable(
                name: "Fnc_HouseType");

            migrationBuilder.DropTable(
                name: "Fnc_Branch");
        }
    }
}
