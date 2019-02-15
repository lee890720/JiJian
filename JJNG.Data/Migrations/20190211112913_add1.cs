using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class add1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
    name: "PK_Brh_Yun",
    table: "Brh_Yun");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Brh_Yun");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brh_Yun",
                table: "Brh_Yun",
                column: "系统订单号");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
