using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class StringForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "InForm");

            migrationBuilder.CreateTable(
                name: "Forms",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Subtitle = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormElementBases",
                schema: "InForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Subtitle = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    ParentFormId = table.Column<long>(type: "bigint", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    RenderAsTextArea = table.Column<bool>(type: "boolean", nullable: true),
                    MaxLength = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormElementBases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormElementBases_Forms_ParentFormId",
                        column: x => x.ParentFormId,
                        principalSchema: "InForm",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormElementBases_ParentFormId",
                schema: "InForm",
                table: "FormElementBases",
                column: "ParentFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormElementBases",
                schema: "InForm");

            migrationBuilder.DropTable(
                name: "Forms",
                schema: "InForm");
        }
    }
}
