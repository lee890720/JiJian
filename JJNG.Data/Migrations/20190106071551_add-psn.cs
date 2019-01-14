using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace JJNG.Data.Migrations
{
    public partial class addpsn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Psn_AddressAccount",
                columns: table => new
                {
                    AddressAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>(nullable: true),
                    BelongTo = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Manager = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psn_AddressAccount", x => x.AddressAccountId);
                });

            migrationBuilder.CreateTable(
                name: "Psn_NoteAccount",
                columns: table => new
                {
                    NoteAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>(nullable: true),
                    BelongTo = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Manager = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psn_NoteAccount", x => x.NoteAccountId);
                });

            migrationBuilder.CreateTable(
                name: "Psn_Address",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressAccountId = table.Column<int>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Purpose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psn_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Psn_Address_Psn_AddressAccount_AddressAccountId",
                        column: x => x.AddressAccountId,
                        principalTable: "Psn_AddressAccount",
                        principalColumn: "AddressAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Psn_Note",
                columns: table => new
                {
                    NoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(nullable: true),
                    Branch = table.Column<string>(nullable: false),
                    EnteringStaff = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    NoteAccountId = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psn_Note", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Psn_Note_Psn_NoteAccount_NoteAccountId",
                        column: x => x.NoteAccountId,
                        principalTable: "Psn_NoteAccount",
                        principalColumn: "NoteAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Psn_Address_AddressAccountId",
                table: "Psn_Address",
                column: "AddressAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Psn_Note_NoteAccountId",
                table: "Psn_Note",
                column: "NoteAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Psn_Address");

            migrationBuilder.DropTable(
                name: "Psn_Note");

            migrationBuilder.DropTable(
                name: "Psn_AddressAccount");

            migrationBuilder.DropTable(
                name: "Psn_NoteAccount");
        }
    }
}
