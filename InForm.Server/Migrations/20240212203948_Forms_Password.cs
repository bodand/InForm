using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class Forms_Password : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "InForm",
                table: "Forms",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "InForm",
                table: "Forms");
        }
    }
}
