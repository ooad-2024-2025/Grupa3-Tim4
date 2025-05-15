using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharityFoundation.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Akcije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Akcije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKorisnika = table.Column<int>(type: "int", nullable: false),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    VrstaPomoci = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DatumDonacije = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donacije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Izvjestaji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mjesec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Godina = table.Column<int>(type: "int", nullable: false),
                    BrojDonacija = table.Column<int>(type: "int", nullable: false),
                    UkupnaVrijednost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izvjestaji", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipKorisnika = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifikacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKorisnika = table.Column<int>(type: "int", nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tip = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifikacije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolonterAkcije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVolontera = table.Column<int>(type: "int", nullable: false),
                    IdAkcije = table.Column<int>(type: "int", nullable: false),
                    StatusUcesca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolonterAkcije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZahtjeviZaPomoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdKorisnika = table.Column<int>(type: "int", nullable: false),
                    OpisPotrebe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Urgentnost = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZahtjeviZaPomoc", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Akcije");

            migrationBuilder.DropTable(
                name: "Donacije");

            migrationBuilder.DropTable(
                name: "Izvjestaji");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Notifikacije");

            migrationBuilder.DropTable(
                name: "VolonterAkcije");

            migrationBuilder.DropTable(
                name: "ZahtjeviZaPomoc");
        }
    }
}
