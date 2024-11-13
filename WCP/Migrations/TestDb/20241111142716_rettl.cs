using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class rettl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reserved",
                table: "Balances",
                newName: "Underway");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1);

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
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Underway",
                table: "Balances",
                newName: "Reserved");
        }
    }
}
