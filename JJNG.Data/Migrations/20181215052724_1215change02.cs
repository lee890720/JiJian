using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class _1215change02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Classify",
                table: "Brh_ExpendRecord",
                newName: "ExpendType");

            migrationBuilder.RenameColumn(
                name: "Classify",
                table: "Brh_EarningRecord",
                newName: "EarningType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpendType",
                table: "Brh_ExpendRecord",
                newName: "Classify");

            migrationBuilder.RenameColumn(
                name: "EarningType",
                table: "Brh_EarningRecord",
                newName: "Classify");
        }
    }
}
