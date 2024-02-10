using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class FillData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "FillDataSequence",
                schema: "InForm");

            migrationBuilder.AlterColumn<int>(
                name: "MaxLength",
                schema: "InForm",
                table: "FormElementBases",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StringFillData",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"InForm\".\"FillDataSequence\"')"),
                    ParentElementId = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringFillData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StringFillData_FormElementBases_ParentElementId",
                        column: x => x.ParentElementId,
                        principalSchema: "InForm",
                        principalTable: "FormElementBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StringFillData_ParentElementId",
                schema: "InForm",
                table: "StringFillData",
                column: "ParentElementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StringFillData",
                schema: "InForm");

            migrationBuilder.DropSequence(
                name: "FillDataSequence",
                schema: "InForm");

            migrationBuilder.AlterColumn<long>(
                name: "MaxLength",
                schema: "InForm",
                table: "FormElementBases",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
