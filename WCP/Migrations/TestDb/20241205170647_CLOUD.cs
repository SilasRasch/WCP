using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class CLOUD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorAudio",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "CreatorVisuals",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "FinalContent",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "OtherFiles",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "FinalContent",
                table: "StaticProjects");

            migrationBuilder.DropColumn(
                name: "CreatorAudio",
                table: "PhotoProjects");

            migrationBuilder.DropColumn(
                name: "CreatorVisuals",
                table: "PhotoProjects");

            migrationBuilder.DropColumn(
                name: "FinalContent",
                table: "PhotoProjects");

            migrationBuilder.DropColumn(
                name: "OtherFiles",
                table: "PhotoProjects");

            migrationBuilder.RenameColumn(
                name: "Scripts",
                table: "UgcProjects",
                newName: "Files");

            migrationBuilder.RenameColumn(
                name: "OtherFiles",
                table: "StaticProjects",
                newName: "Files");

            migrationBuilder.RenameColumn(
                name: "Scripts",
                table: "PhotoProjects",
                newName: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Files",
                table: "UgcProjects",
                newName: "Scripts");

            migrationBuilder.RenameColumn(
                name: "Files",
                table: "StaticProjects",
                newName: "OtherFiles");

            migrationBuilder.RenameColumn(
                name: "Files",
                table: "PhotoProjects",
                newName: "Scripts");

            migrationBuilder.AddColumn<string>(
                name: "CreatorAudio",
                table: "UgcProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorVisuals",
                table: "UgcProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinalContent",
                table: "UgcProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherFiles",
                table: "UgcProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinalContent",
                table: "StaticProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorAudio",
                table: "PhotoProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorVisuals",
                table: "PhotoProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinalContent",
                table: "PhotoProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherFiles",
                table: "PhotoProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
