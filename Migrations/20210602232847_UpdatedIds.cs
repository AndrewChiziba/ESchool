using Microsoft.EntityFrameworkCore.Migrations;

namespace ESchool.Migrations
{
    public partial class UpdatedIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Teachers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Students",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Teachers",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "ID");
        }
    }
}
