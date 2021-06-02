using Microsoft.EntityFrameworkCore.Migrations;

namespace ESchool.Migrations
{
    public partial class newID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherID",
                table: "TimeTables",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "TimeTables",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Teachers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Teachers",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Students",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Students",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Results",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "ExerciseID",
                table: "Results",
                newName: "ExerciseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "TimeTables",
                newName: "TeacherID");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "TimeTables",
                newName: "CourseID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Teachers",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Teachers",
                newName: "CourseID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Students",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Students",
                newName: "CourseID");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Results",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "Results",
                newName: "ExerciseID");
        }
    }
}
