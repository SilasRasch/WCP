using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class CreatorParticipations3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations");

            migrationBuilder.DropIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations",
                columns: new[] { "OrderId", "CreatorId" });
        }
    }
}
