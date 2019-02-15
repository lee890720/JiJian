using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class brhyun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brh_Yun",
                columns: table => new
                {
                    系统订单号 = table.Column<long>(nullable: false),
                    到店时间 = table.Column<DateTime>(nullable: false),
                    客人 = table.Column<string>(nullable: true),
                    房费 = table.Column<decimal>(nullable: false),
                    房间号 = table.Column<string>(nullable: true),
                    订单备注 = table.Column<string>(nullable: true),
                    订单来源 = table.Column<string>(nullable: true),
                    订单状态 = table.Column<string>(nullable: true),
                    退房时间 = table.Column<DateTime>(nullable: false),
                    间夜 = table.Column<int>(nullable: false),
                    预订日期 = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brh_Yun", x => x.系统订单号);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brh_Yun");
        }
    }
}
