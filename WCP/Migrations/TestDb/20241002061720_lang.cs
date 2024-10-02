using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class lang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "StaticTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LanguageId",
                table: "Users",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_LanguageId",
                table: "Organizations",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Languages_LanguageId",
                table: "Organizations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Languages_LanguageId",
                table: "Users",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Languages_LanguageId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Languages_LanguageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LanguageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_LanguageId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "StaticTemplates");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Organizations");
        }
    }
}
