using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCPShared.Migrations.TestDb
{
    /// <inheritdoc />
    public partial class concepts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoProjects");

            migrationBuilder.DropTable(
                name: "StaticProjectStaticTemplate");

            migrationBuilder.DropTable(
                name: "UgcProjects");

            migrationBuilder.DropTable(
                name: "StaticProjects");

            migrationBuilder.DropSequence(
                name: "ProjectSequence");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Files = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    CreatorLanguageId = table.Column<int>(type: "int", nullable: true),
                    CreatorGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorAge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorCount = table.Column<int>(type: "int", nullable: true),
                    CreativesPerCreator = table.Column<int>(type: "int", nullable: true),
                    CreatorKeepsProduct = table.Column<bool>(type: "bit", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Languages_CreatorLanguageId",
                        column: x => x.CreatorLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Concepts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Formats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatorLanguageId = table.Column<int>(type: "int", nullable: true),
                    CreatorGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorAge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorCount = table.Column<int>(type: "int", nullable: true),
                    CreativesPerCreator = table.Column<int>(type: "int", nullable: true),
                    CreatorKeepsProduct = table.Column<bool>(type: "bit", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LengthInSeconds = table.Column<int>(type: "int", nullable: true),
                    ExtraHooks = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concepts_Languages_CreatorLanguageId",
                        column: x => x.CreatorLanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Concepts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Concepts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "StaticConceptStaticTemplate",
                columns: table => new
                {
                    ConceptsId = table.Column<int>(type: "int", nullable: false),
                    StaticTemplatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticConceptStaticTemplate", x => new { x.ConceptsId, x.StaticTemplatesId });
                    table.ForeignKey(
                        name: "FK_StaticConceptStaticTemplate_Concepts_ConceptsId",
                        column: x => x.ConceptsId,
                        principalTable: "Concepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaticConceptStaticTemplate_StaticTemplates_StaticTemplatesId",
                        column: x => x.StaticTemplatesId,
                        principalTable: "StaticTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_CreatorLanguageId",
                table: "Concepts",
                column: "CreatorLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_ProductId",
                table: "Concepts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Concepts_ProjectId",
                table: "Concepts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BrandId",
                table: "Projects",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatorLanguageId",
                table: "Projects",
                column: "CreatorLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticConceptStaticTemplate_StaticTemplatesId",
                table: "StaticConceptStaticTemplate",
                column: "StaticTemplatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreatorParticipations_Projects_ProjectId",
                table: "CreatorParticipations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Projects_ProjectId",
                table: "Feedbacks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreatorParticipations_Projects_ProjectId",
                table: "CreatorParticipations");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Projects_ProjectId",
                table: "Feedbacks");

            migrationBuilder.DropTable(
                name: "StaticConceptStaticTemplate");

            migrationBuilder.DropTable(
                name: "Concepts");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.CreateSequence(
                name: "ProjectSequence");

            migrationBuilder.CreateTable(
                name: "PhotoProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ProjectSequence]"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Files = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Formats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorLanguageId = table.Column<int>(type: "int", nullable: false),
                    CreativesPerCreator = table.Column<int>(type: "int", nullable: false),
                    CreatorAge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorBudget = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorCount = table.Column<int>(type: "int", nullable: false),
                    CreatorGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorKeepsProduct = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Files = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Formats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Files = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Formats = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorLanguageId = table.Column<int>(type: "int", nullable: false),
                    CreativesPerCreator = table.Column<int>(type: "int", nullable: false),
                    CreatorAge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorBudget = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorCount = table.Column<int>(type: "int", nullable: false),
                    CreatorGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorKeepsProduct = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtraHooks = table.Column<int>(type: "int", nullable: false),
                    LengthInSeconds = table.Column<int>(type: "int", nullable: false)
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
        }
    }
}
