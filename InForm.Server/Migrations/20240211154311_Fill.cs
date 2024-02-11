using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class Fill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FillId",
                schema: "InForm",
                table: "StringFillData",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Fill",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fill", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StringFillData_FillId",
                schema: "InForm",
                table: "StringFillData",
                column: "FillId");

            migrationBuilder.AddForeignKey(
                name: "FK_StringFillData_Fill_FillId",
                schema: "InForm",
                table: "StringFillData",
                column: "FillId",
                principalSchema: "InForm",
                principalTable: "Fill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StringFillData_Fill_FillId",
                schema: "InForm",
                table: "StringFillData");

            migrationBuilder.DropTable(
                name: "Fill",
                schema: "InForm");

            migrationBuilder.DropIndex(
                name: "IX_StringFillData_FillId",
                schema: "InForm",
                table: "StringFillData");

            migrationBuilder.DropColumn(
                name: "FillId",
                schema: "InForm",
                table: "StringFillData");
        }
    }
}
