using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class creatorslots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EditorId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideographerId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EditorId",
                table: "Orders",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VideographerId",
                table: "Orders",
                column: "VideographerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Creators_EditorId",
                table: "Orders",
                column: "EditorId",
                principalTable: "Creators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Creators_VideographerId",
                table: "Orders",
                column: "VideographerId",
                principalTable: "Creators",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Creators_EditorId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Creators_VideographerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_EditorId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VideographerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VideographerId",
                table: "Orders");
        }
    }
}
