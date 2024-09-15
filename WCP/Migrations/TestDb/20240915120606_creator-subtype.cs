using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class creatorsubtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditor",
                table: "Creators");

            migrationBuilder.AddColumn<string>(
                name: "SubType",
                table: "Creators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "UGC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubType",
                table: "Creators");

            migrationBuilder.AddColumn<bool>(
                name: "IsEditor",
                table: "Creators",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
