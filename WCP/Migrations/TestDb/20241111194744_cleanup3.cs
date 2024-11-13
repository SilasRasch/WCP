using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class cleanup3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Creators_Balances_BalanceId",
                table: "Creators");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Balances");

            migrationBuilder.DropIndex(
                name: "IX_Creators_BalanceId",
                table: "Creators");

            migrationBuilder.DropColumn(
                name: "BalanceId",
                table: "Creators");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "CreatorParticipations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "CreatorParticipations");

            migrationBuilder.AddColumn<int>(
                name: "BalanceId",
                table: "Creators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pending = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Underway = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BalanceId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Balances_BalanceId",
                        column: x => x.BalanceId,
                        principalTable: "Balances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Creators_BalanceId",
                table: "Creators",
                column: "BalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BalanceId",
                table: "Transactions",
                column: "BalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Creators_Balances_BalanceId",
                table: "Creators",
                column: "BalanceId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
