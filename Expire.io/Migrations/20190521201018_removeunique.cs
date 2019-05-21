using Microsoft.EntityFrameworkCore.Migrations;

namespace Expire.io.Migrations
{
    public partial class removeunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Document_Name",
                table: "Document");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Document_Name",
                table: "Document",
                column: "Name",
                unique: true);
        }
    }
}
