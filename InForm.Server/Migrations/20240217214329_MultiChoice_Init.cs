using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class MultiChoice_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                schema: "InForm",
                table: "FormElementBases",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<long>(
                name: "FillDataId",
                schema: "InForm",
                table: "FormElementBases",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxSelected",
                schema: "InForm",
                table: "FormElementBases",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MultiChoiceFillDatas",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"InForm\".\"FillDataSequence\"')"),
                    FillId = table.Column<long>(type: "bigint", nullable: false),
                    ParentElementId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiChoiceFillDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultiChoiceFillDatas_Fill_FillId",
                        column: x => x.FillId,
                        principalSchema: "InForm",
                        principalTable: "Fill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MultiChoiceFillDatas_FormElementBases_ParentElementId",
                        column: x => x.ParentElementId,
                        principalSchema: "InForm",
                        principalTable: "FormElementBases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MultiChoiceOptions",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ElementId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiChoiceOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultiChoiceOptions_FormElementBases_ElementId",
                        column: x => x.ElementId,
                        principalSchema: "InForm",
                        principalTable: "FormElementBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiChoiceFillSelections",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FillDataId = table.Column<long>(type: "bigint", nullable: false),
                    OptionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiChoiceFillSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultiChoiceFillSelections_MultiChoiceFillDatas_FillDataId",
                        column: x => x.FillDataId,
                        principalSchema: "InForm",
                        principalTable: "MultiChoiceFillDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MultiChoiceFillSelections_MultiChoiceOptions_OptionId",
                        column: x => x.OptionId,
                        principalSchema: "InForm",
                        principalTable: "MultiChoiceOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormElementBases_FillDataId",
                schema: "InForm",
                table: "FormElementBases",
                column: "FillDataId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiChoiceFillDatas_FillId",
                schema: "InForm",
                table: "MultiChoiceFillDatas",
                column: "FillId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiChoiceFillDatas_ParentElementId",
                schema: "InForm",
                table: "MultiChoiceFillDatas",
                column: "ParentElementId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiChoiceFillSelections_FillDataId",
                schema: "InForm",
                table: "MultiChoiceFillSelections",
                column: "FillDataId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiChoiceFillSelections_OptionId",
                schema: "InForm",
                table: "MultiChoiceFillSelections",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_MultiChoiceOptions_ElementId",
                schema: "InForm",
                table: "MultiChoiceOptions",
                column: "ElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormElementBases_MultiChoiceFillDatas_FillDataId",
                schema: "InForm",
                table: "FormElementBases",
                column: "FillDataId",
                principalSchema: "InForm",
                principalTable: "MultiChoiceFillDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormElementBases_MultiChoiceFillDatas_FillDataId",
                schema: "InForm",
                table: "FormElementBases");

            migrationBuilder.DropTable(
                name: "MultiChoiceFillSelections",
                schema: "InForm");

            migrationBuilder.DropTable(
                name: "MultiChoiceFillDatas",
                schema: "InForm");

            migrationBuilder.DropTable(
                name: "MultiChoiceOptions",
                schema: "InForm");

            migrationBuilder.DropIndex(
                name: "IX_FormElementBases_FillDataId",
                schema: "InForm",
                table: "FormElementBases");

            migrationBuilder.DropColumn(
                name: "FillDataId",
                schema: "InForm",
                table: "FormElementBases");

            migrationBuilder.DropColumn(
                name: "MaxSelected",
                schema: "InForm",
                table: "FormElementBases");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                schema: "InForm",
                table: "FormElementBases",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(34)",
                oldMaxLength: 34);
        }
    }
}
