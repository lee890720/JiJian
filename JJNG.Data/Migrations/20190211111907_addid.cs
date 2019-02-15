using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class addid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Brh_Yun",
                table: "Brh_Yun");

            migrationBuilder.AddColumn<int>(
                name: "系统订单号2",
                table: "Brh_Yun",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brh_Yun",
                table: "Brh_Yun",
                column: "系统订单号2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Brh_Yun",
                table: "Brh_Yun");

            migrationBuilder.DropColumn(
                name: "系统订单号2",
                table: "Brh_Yun");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brh_Yun",
                table: "Brh_Yun",
                column: "系统订单号");
        }
    }
}
