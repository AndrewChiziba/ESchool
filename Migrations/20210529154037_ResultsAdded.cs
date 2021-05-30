using Microsoft.EntityFrameworkCore.Migrations;

namespace ESchool.Migrations
{
    public partial class ResultsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalMark",
                table: "Results",
                newName: "TotalScore");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Results",
                newName: "StudentScore");

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Results",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "TotalScore",
                table: "Results",
                newName: "TotalMark");

            migrationBuilder.RenameColumn(
                name: "StudentScore",
                table: "Results",
                newName: "CourseID");
        }
    }
}
