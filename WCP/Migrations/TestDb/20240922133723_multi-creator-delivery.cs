using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class multicreatordelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatorDeliveryStatus",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatorDeliveryStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Orders");
        }
    }
}
