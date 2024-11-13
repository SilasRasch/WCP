using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class langcodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IsoCountryCode",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IsoLanguageCode",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "IsoCountryCode",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "IsoLanguageCode",
                table: "Languages");
        }
    }
}
