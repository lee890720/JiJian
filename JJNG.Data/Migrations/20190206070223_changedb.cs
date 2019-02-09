using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class changedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BelongTo",
                table: "Psn_NoteAccount",
                newName: "Branch");

            migrationBuilder.RenameColumn(
                name: "BelongTo",
                table: "Psn_AddressAccount",
                newName: "Branch");

            migrationBuilder.RenameColumn(
                name: "BelongTo",
                table: "Brh_ImprestAccounts",
                newName: "Branch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Branch",
                table: "Psn_NoteAccount",
                newName: "BelongTo");

            migrationBuilder.RenameColumn(
                name: "Branch",
                table: "Psn_AddressAccount",
                newName: "BelongTo");

            migrationBuilder.RenameColumn(
                name: "Branch",
                table: "Brh_ImprestAccounts",
                newName: "BelongTo");
        }
    }
}
