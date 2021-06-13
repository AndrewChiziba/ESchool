using Microsoft.EntityFrameworkCore.Migrations;

namespace ESchool.Migrations
{
    public partial class TimetableWorks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "TimeTables",
                newName: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "TimeTables",
                newName: "CourseName");
        }
    }
}
