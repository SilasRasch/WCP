using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class CreatorParticipations4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations");

            migrationBuilder.DropIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CreatorParticipations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations",
                columns: new[] { "OrderId", "CreatorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CreatorParticipations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations",
                column: "OrderId");
        }
    }
}
