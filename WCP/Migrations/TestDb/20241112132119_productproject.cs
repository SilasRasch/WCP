using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class productproject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorParticipations_Orders_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Brands_BrandId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Creators_EditorId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Creators_VideographerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticTemplates_Orders_OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_VideographerId",
                table: "Order",
                newName: "IX_Order_VideographerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_EditorId",
                table: "Order",
                newName: "IX_Order_EditorId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BrandId",
                table: "Order",
                newName: "IX_Order_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorParticipations_Order_OrderId",
                table: "CreatorParticipations",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Brands_BrandId",
                table: "Order",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Creators_EditorId",
                table: "Order",
                column: "EditorId",
                principalTable: "Creators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Creators_VideographerId",
                table: "Order",
                column: "VideographerId",
                principalTable: "Creators",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorParticipations_Order_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Brands_BrandId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Creators_EditorId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Creators_VideographerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Order_OrderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticTemplates_Order_OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_VideographerId",
                table: "Orders",
                newName: "IX_Orders_VideographerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_EditorId",
                table: "Orders",
                newName: "IX_Orders_EditorId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_BrandId",
                table: "Orders",
                newName: "IX_Orders_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorParticipations_Orders_OrderId",
                table: "CreatorParticipations",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Brands_BrandId",
                table: "Orders",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StaticTemplates_Orders_OrderId",
                table: "StaticTemplates",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
