using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharityFoundation.Migrations
{
    /// <inheritdoc />
    public partial class InitialRoleBasedStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpisPotrebe",
                table: "ZahtjeviZaPomoc",
                newName: "Opis");

            migrationBuilder.AddColumn<DateTime>(
                name: "Datum",
                table: "ZahtjeviZaPomoc",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Datum",
                table: "Izvjestaji",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "Izvjestaji",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Opis",
                table: "Izvjestaji",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Datum",
                table: "ZahtjeviZaPomoc");

            migrationBuilder.DropColumn(
                name: "Datum",
                table: "Izvjestaji");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Izvjestaji");

            migrationBuilder.DropColumn(
                name: "Opis",
                table: "Izvjestaji");

            migrationBuilder.RenameColumn(
                name: "Opis",
                table: "ZahtjeviZaPomoc",
                newName: "OpisPotrebe");
        }
    }
}
