using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class NumericRange_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxRange",
                schema: "InForm",
                table: "FormElementBases",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinRange",
                schema: "InForm",
                table: "FormElementBases",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RangeElementQuestion",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RangeId = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangeElementQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RangeElementQuestion_FormElementBases_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "InForm",
                        principalTable: "FormElementBases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NumericRangeFillData",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"InForm\".\"FillDataSequence\"')"),
                    FillId = table.Column<long>(type: "bigint", nullable: false),
                    ParnetElementId = table.Column<long>(type: "bigint", nullable: false),
                    RangeElementQuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ParentElementId = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumericRangeFillData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumericRangeFillData_Fill_FillId",
                        column: x => x.FillId,
                        principalSchema: "InForm",
                        principalTable: "Fill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NumericRangeFillData_FormElementBases_ParentElementId",
                        column: x => x.ParentElementId,
                        principalSchema: "InForm",
                        principalTable: "FormElementBases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NumericRangeFillData_RangeElementQuestion_RangeElementQuest~",
                        column: x => x.RangeElementQuestionId,
                        principalSchema: "InForm",
                        principalTable: "RangeElementQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NumericRangeFillData_FillId",
                schema: "InForm",
                table: "NumericRangeFillData",
                column: "FillId");

            migrationBuilder.CreateIndex(
                name: "IX_NumericRangeFillData_ParentElementId",
                schema: "InForm",
                table: "NumericRangeFillData",
                column: "ParentElementId");

            migrationBuilder.CreateIndex(
                name: "IX_NumericRangeFillData_RangeElementQuestionId",
                schema: "InForm",
                table: "NumericRangeFillData",
                column: "RangeElementQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RangeElementQuestion_ParentId",
                schema: "InForm",
                table: "RangeElementQuestion",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumericRangeFillData",
                schema: "InForm");

            migrationBuilder.DropTable(
                name: "RangeElementQuestion",
                schema: "InForm");

            migrationBuilder.DropColumn(
                name: "MaxRange",
                schema: "InForm",
                table: "FormElementBases");

            migrationBuilder.DropColumn(
                name: "MinRange",
                schema: "InForm",
                table: "FormElementBases");
        }
    }
}
