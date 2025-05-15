using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharityFoundation.Migrations
{
    /// <inheritdoc />
    public partial class FixVolonterAkcijaForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdAkcije",
                table: "VolonterAkcije",
                newName: "AkcijaID");

            migrationBuilder.CreateIndex(
                name: "IX_VolonterAkcije_AkcijaID",
                table: "VolonterAkcije",
                column: "AkcijaID");

            migrationBuilder.AddForeignKey(
                name: "FK_VolonterAkcije_Akcije_AkcijaID",
                table: "VolonterAkcije",
                column: "AkcijaID",
                principalTable: "Akcije",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolonterAkcije_Akcije_AkcijaID",
                table: "VolonterAkcije");

            migrationBuilder.DropIndex(
                name: "IX_VolonterAkcije_AkcijaID",
                table: "VolonterAkcije");

            migrationBuilder.RenameColumn(
                name: "AkcijaID",
                table: "VolonterAkcije",
                newName: "IdAkcije");
        }
    }
}
