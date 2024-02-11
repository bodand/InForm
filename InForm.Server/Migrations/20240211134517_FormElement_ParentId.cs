using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class FormElement_ParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentFormId",
                schema: "InForm",
                table: "FormElementBases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentFormId",
                schema: "InForm",
                table: "FormElementBases",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
