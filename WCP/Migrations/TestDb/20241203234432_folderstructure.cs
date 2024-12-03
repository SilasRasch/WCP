using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class folderstructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorContent",
                table: "UgcProjects",
                newName: "CreatorVisuals");

            migrationBuilder.RenameColumn(
                name: "CreatorContent",
                table: "PhotoProjects",
                newName: "CreatorVisuals");

            migrationBuilder.AddColumn<string>(
                name: "CreatorAudio",
                table: "UgcProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorAudio",
                table: "PhotoProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorAudio",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "CreatorAudio",
                table: "PhotoProjects");

            migrationBuilder.RenameColumn(
                name: "CreatorVisuals",
                table: "UgcProjects",
                newName: "CreatorContent");

            migrationBuilder.RenameColumn(
                name: "CreatorVisuals",
                table: "PhotoProjects",
                newName: "CreatorContent");
        }
    }
}
