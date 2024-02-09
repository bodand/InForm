using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class FormGuidId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdGuid",
                schema: "InForm",
                table: "Forms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdGuid",
                schema: "InForm",
                table: "Forms");
        }
    }
}
