using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fnc_Branch",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchName = table.Column<string>(nullable: true),
                    IsType = table.Column<bool>(nullable: false)
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
                    HouseType = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true)
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
                    HouseNumberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                name: "App_Menu");

            migrationBuilder.DropTable(
                name: "Brh_Client");

            migrationBuilder.DropTable(
                name: "Brh_ConnectRecord");

            migrationBuilder.DropTable(
                name: "Brh_EarningRecord");

            migrationBuilder.DropTable(
                name: "Brh_ExpendRecord");

            migrationBuilder.DropTable(
                name: "Brh_FrontPaymentDetial");

            migrationBuilder.DropTable(
                name: "Brh_FrontPaymentDetial2");

            migrationBuilder.DropTable(
                name: "Brh_ImprestRecord");

            migrationBuilder.DropTable(
                name: "Brh_Memo");

            migrationBuilder.DropTable(
                name: "Brh_StewardPaymentDetial");

            migrationBuilder.DropTable(
                name: "Brh_Yun");

            migrationBuilder.DropTable(
                name: "Fnc_ChannelType");

            migrationBuilder.DropTable(
                name: "Fnc_EarningType");

            migrationBuilder.DropTable(
                name: "Fnc_ExpendType");

            migrationBuilder.DropTable(
                name: "Fnc_HouseNumber");

            migrationBuilder.DropTable(
                name: "Fnc_PaymentType");

            migrationBuilder.DropTable(
                name: "Psn_Address");

            migrationBuilder.DropTable(
                name: "Psn_Note");

            migrationBuilder.DropTable(
                name: "Brh_FrontDeskAccounts");

            migrationBuilder.DropTable(
                name: "Brh_ImprestAccounts");

            migrationBuilder.DropTable(
                name: "Brh_StewardAccounts");

            migrationBuilder.DropTable(
                name: "Fnc_HouseType");

            migrationBuilder.DropTable(
                name: "Psn_AddressAccount");

            migrationBuilder.DropTable(
                name: "Psn_NoteAccount");

            migrationBuilder.DropTable(
                name: "Fnc_Branch");
        }
    }
}
