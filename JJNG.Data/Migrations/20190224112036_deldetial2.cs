using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class deldetial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brh_FrontPaymentDetial2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brh_FrontPaymentDetial2",
                columns: table => new
                {
                    FrontPaymentDetialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FrontDeskAccountsId = table.Column<long>(nullable: false),
                    PayAmount = table.Column<decimal>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: false),
                    PayWay = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_FrontPaymentDetial2", x => x.FrontPaymentDetialId);
                });
        }
    }
}
