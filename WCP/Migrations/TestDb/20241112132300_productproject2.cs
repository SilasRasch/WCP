using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class productproject2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorParticipations_Order_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Order_OrderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticTemplates_Order_OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropIndex(
                name: "IX_StaticTemplates_OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CreatorParticipations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "StaticTemplates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "CreatorParticipations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    EditorId = table.Column<int>(type: "int", nullable: true),
                    VideographerId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentCount = table.Column<int>(type: "int", nullable: false),
                    ContentLength = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Delivery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryTimeFrom = table.Column<int>(type: "int", nullable: false),
                    DeliveryTimeTo = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraCreator = table.Column<bool>(type: "bit", nullable: true),
                    ExtraHook = table.Column<int>(type: "int", nullable: true),
                    ExtraNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FocusPoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ideas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelevantFiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scripts = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Creators_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Creators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Creators_VideographerId",
                        column: x => x.VideographerId,
                        principalTable: "Creators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaticTemplates_OrderId",
                table: "StaticTemplates",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId",
                table: "Products",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_BrandId",
                table: "Order",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EditorId",
                table: "Order",
                column: "EditorId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_VideographerId",
                table: "Order",
                column: "VideographerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorParticipations_Order_OrderId",
                table: "CreatorParticipations",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Order_OrderId",
                table: "Products",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaticTemplates_Order_OrderId",
                table: "StaticTemplates",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
