using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations
{
    /// <inheritdoc />
    public partial class statics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaticTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateImgOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateImgTwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExampleImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStaticTemplate",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    StaticTemplatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStaticTemplate", x => new { x.OrdersId, x.StaticTemplatesId });
                    table.ForeignKey(
                        name: "FK_OrderStaticTemplate_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderStaticTemplate_StaticTemplates_StaticTemplatesId",
                        column: x => x.StaticTemplatesId,
                        principalTable: "StaticTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStaticTemplate_StaticTemplatesId",
                table: "OrderStaticTemplate",
                column: "StaticTemplatesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStaticTemplate");

            migrationBuilder.DropTable(
                name: "StaticTemplates");
        }
    }
}
