using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class Stripe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Organizations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PriceEstimate",
                table: "Creators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StripeAccountId",
                table: "Creators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "PriceEstimate",
                table: "Creators");

            migrationBuilder.DropColumn(
                name: "StripeAccountId",
                table: "Creators");
        }
    }
}
