using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class project20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorParticipations_Orders_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropTable(
                name: "OrderStaticTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations");

            migrationBuilder.DropColumn(
                name: "Products",
                table: "Orders");

            migrationBuilder.CreateSequence(
                name: "ProjectSequence");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "StaticTemplates",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "CreatorParticipations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "CreatorParticipations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations",
                columns: new[] { "ProjectId", "CreatorId" });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pains = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Features = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FocusPoints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HowToUse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PhotoProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ProjectSequence]"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Formats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatorLanguageId = table.Column<int>(type: "int", nullable: false),
                    CreatorGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorAge = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CreatorBudget = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CreatorCount = table.Column<int>(type: "int", nullable: false),
                    CreativesPerCreator = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorKeepsProduct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoProjects_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoProjects_Languages_CreatorLanguageId",
                        column: x => x.CreatorLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoProjects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaticProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ProjectSequence]"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Formats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticProjects_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaticProjects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UgcProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ProjectSequence]"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Formats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LengthInSeconds = table.Column<int>(type: "int", nullable: false),
                    ExtraHooks = table.Column<int>(type: "int", nullable: false),
                    CreatorLanguageId = table.Column<int>(type: "int", nullable: false),
                    CreatorGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorAge = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CreatorBudget = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CreatorCount = table.Column<int>(type: "int", nullable: false),
                    CreativesPerCreator = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorKeepsProduct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UgcProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UgcProjects_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgcProjects_Languages_CreatorLanguageId",
                        column: x => x.CreatorLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UgcProjects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaticProjectStaticTemplate",
                columns: table => new
                {
                    ProjectsId = table.Column<int>(type: "int", nullable: false),
                    StaticTemplatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticProjectStaticTemplate", x => new { x.ProjectsId, x.StaticTemplatesId });
                    table.ForeignKey(
                        name: "FK_StaticProjectStaticTemplate_StaticProjects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "StaticProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaticProjectStaticTemplate_StaticTemplates_StaticTemplatesId",
                        column: x => x.StaticTemplatesId,
                        principalTable: "StaticTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaticTemplates_OrderId",
                table: "StaticTemplates",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoProjects_BrandId",
                table: "PhotoProjects",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoProjects_CreatorLanguageId",
                table: "PhotoProjects",
                column: "CreatorLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoProjects_ProductId",
                table: "PhotoProjects",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId",
                table: "Products",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticProjects_BrandId",
                table: "StaticProjects",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticProjects_ProductId",
                table: "StaticProjects",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticProjectStaticTemplate_StaticTemplatesId",
                table: "StaticProjectStaticTemplate",
                column: "StaticTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_UgcProjects_BrandId",
                table: "UgcProjects",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_UgcProjects_CreatorLanguageId",
                table: "UgcProjects",
                column: "CreatorLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UgcProjects_ProductId",
                table: "UgcProjects",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorParticipations_Orders_OrderId",
                table: "CreatorParticipations",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorParticipations_Orders_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticTemplates_Orders_OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropTable(
                name: "PhotoProjects");

            migrationBuilder.DropTable(
                name: "StaticProjectStaticTemplate");

            migrationBuilder.DropTable(
                name: "UgcProjects");

            migrationBuilder.DropTable(
                name: "StaticProjects");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_StaticTemplates_OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations");

            migrationBuilder.DropIndex(
                name: "IX_CreatorParticipations_OrderId",
                table: "CreatorParticipations");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "StaticTemplates");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CreatorParticipations");

            migrationBuilder.DropSequence(
                name: "ProjectSequence");

            migrationBuilder.AddColumn<string>(
                name: "Products",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "CreatorParticipations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreatorParticipations",
                table: "CreatorParticipations",
                columns: new[] { "OrderId", "CreatorId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorParticipations_Orders_OrderId",
                table: "CreatorParticipations",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
