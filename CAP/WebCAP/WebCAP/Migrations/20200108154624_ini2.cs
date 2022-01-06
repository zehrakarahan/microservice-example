using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCAP.Migrations
{
    public partial class ini2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatuName",
                table: "cap.received",
                newName: "StatusName");

            migrationBuilder.RenameColumn(
                name: "StatuName",
                table: "cap.published",
                newName: "StatusName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "cap.received",
                newName: "StatuName");

            migrationBuilder.RenameColumn(
                name: "StatusName",
                table: "cap.published",
                newName: "StatuName");
        }
    }
}
