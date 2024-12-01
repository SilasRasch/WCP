using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class files2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorContent",
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
                name: "Scripts",
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
                name: "OtherFiles",
                table: "StaticProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorContent",
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

            migrationBuilder.AddColumn<string>(
                name: "Scripts",
                table: "PhotoProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorContent",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "FinalContent",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "OtherFiles",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "Scripts",
                table: "UgcProjects");

            migrationBuilder.DropColumn(
                name: "FinalContent",
                table: "StaticProjects");

            migrationBuilder.DropColumn(
                name: "OtherFiles",
                table: "StaticProjects");

            migrationBuilder.DropColumn(
                name: "CreatorContent",
                table: "PhotoProjects");

            migrationBuilder.DropColumn(
                name: "FinalContent",
                table: "PhotoProjects");

            migrationBuilder.DropColumn(
                name: "OtherFiles",
                table: "PhotoProjects");

            migrationBuilder.DropColumn(
                name: "Scripts",
                table: "PhotoProjects");
        }
    }
}
