using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations.AppIdentityDb
{
    public partial class changeidentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "User_BelongToDetial");

            //migrationBuilder.DropTable(
            //    name: "User_BelongTo");

            migrationBuilder.DropColumn(
                name: "BelongTo",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "User_Branch",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Branch", x => x.BranchId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_BranchDetial");

            migrationBuilder.DropTable(
                name: "User_Branch");

            migrationBuilder.AddColumn<string>(
                name: "BelongTo",
                table: "AspNetUsers",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "User_BelongTo",
            //    columns: table => new
            //    {
            //        BelongToId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        BelongToName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_User_BelongTo", x => x.BelongToId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "User_BelongToDetial",
            //    columns: table => new
            //    {
            //        BelongToDetialId = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        BelongToId = table.Column<int>(nullable: false),
            //        HouseNumber = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_User_BelongToDetial", x => x.BelongToDetialId);
            //        table.ForeignKey(
            //            name: "FK_User_BelongToDetial_User_BelongTo_BelongToId",
            //            column: x => x.BelongToId,
            //            principalTable: "User_BelongTo",
            //            principalColumn: "BelongToId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_User_BelongToDetial_BelongToId",
            //    table: "User_BelongToDetial",
            //    column: "BelongToId");
        }
    }
}
