using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class change121401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Brh_ConnectRecord",
                columns: table => new
                {
                    ConnectRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillCount = table.Column<int>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    CardCount = table.Column<int>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    HouseCash = table.Column<double>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    OtherCash = table.Column<double>(nullable: false),
                    RevolvingFund = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_ConnectRecord", x => x.ConnectRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Fnc_Payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentName = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fnc_Payment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brh_FrontPaymentDetial_BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial",
                column: "BrhConnectRecordConnectRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brh_FrontPaymentDetial_Brh_ConnectRecord_BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial",
                column: "BrhConnectRecordConnectRecordId",
                principalTable: "Brh_ConnectRecord",
                principalColumn: "ConnectRecordId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brh_FrontPaymentDetial_Brh_ConnectRecord_BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial");

            migrationBuilder.DropTable(
                name: "Brh_ConnectRecord");

            migrationBuilder.DropTable(
                name: "Fnc_Payment");

            migrationBuilder.DropIndex(
                name: "IX_Brh_FrontPaymentDetial_BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial");

            migrationBuilder.DropColumn(
                name: "BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial");
        }
    }
}
