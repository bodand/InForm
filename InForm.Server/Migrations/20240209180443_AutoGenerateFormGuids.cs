using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InForm.Server.Migrations
{
    /// <inheritdoc />
    public partial class AutoGenerateFormGuids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR REPLACE FUNCTION "TR_FormGenerateGuid"() RETURNS TRIGGER
                    PARALLEL SAFE
                    LEAKPROOF
                    LANGUAGE "plpgsql"
                AS
                $$
                BEGIN
                    NEW."IdGuid" := gen_random_uuid();
                    RETURN NEW;
                END;
                $$;
                """);
            migrationBuilder.Sql("""
                CREATE OR REPLACE TRIGGER "TR_GenerateGuidId"
                BEFORE INSERT ON "InForm"."Forms"
                FOR EACH ROW
                EXECUTE FUNCTION "TR_FormGenerateGuid"();
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS "TR_GenerateGuidId" ON "Forms";
                """);
            migrationBuilder.Sql("""
                DROP FUNCTION IF EXISTS "TR_FormGenerateGuid"();
                """);
        }
    }
}
