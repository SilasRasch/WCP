using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class creatorbalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BalanceId",
                table: "Creators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Creators_BalanceId",
                table: "Creators",
                column: "BalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Creators_Balances_BalanceId",
                table: "Creators",
                column: "BalanceId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Creators_Balances_BalanceId",
                table: "Creators");

            migrationBuilder.DropIndex(
                name: "IX_Creators_BalanceId",
                table: "Creators");

            migrationBuilder.DropColumn(
                name: "BalanceId",
                table: "Creators");
        }
    }
}
