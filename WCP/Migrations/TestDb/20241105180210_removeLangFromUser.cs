using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class removeLangFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Languages_LanguageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LanguageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Speciality",
                table: "Creators");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Speciality",
                table: "Creators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LanguageId",
                table: "Users",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Languages_LanguageId",
                table: "Users",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
