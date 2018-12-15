using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class changebrh01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brh_FrontPaymentDetial_Brh_ConnectRecord_BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial");

            migrationBuilder.DropIndex(
                name: "IX_Brh_FrontPaymentDetial_BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial");

            migrationBuilder.DropColumn(
                name: "BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial");

            migrationBuilder.CreateTable(
                name: "Brh_EarningRecord",
                columns: table => new
                {
                    EarningRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    Classify = table.Column<string>(nullable: true),
                    ConnectNumber = table.Column<string>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    PaymentType = table.Column<string>(nullable: false),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_EarningRecord", x => x.EarningRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Brh_ExpendRecord",
                columns: table => new
                {
                    ExpendRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    Classify = table.Column<string>(nullable: true),
                    ConnectNumber = table.Column<string>(nullable: false),
                    EnteringDate = table.Column<DateTime>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    PaymentType = table.Column<string>(nullable: false),
                    Purpose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_ExpendRecord", x => x.ExpendRecordId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brh_EarningRecord");

            migrationBuilder.DropTable(
                name: "Brh_ExpendRecord");

            migrationBuilder.AddColumn<int>(
                name: "BrhConnectRecordConnectRecordId",
                table: "Brh_FrontPaymentDetial",
                nullable: true);

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
    }
}
