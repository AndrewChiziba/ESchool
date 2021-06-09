using Microsoft.EntityFrameworkCore.Migrations;

namespace ESchool.Migrations
{
    public partial class AddedTranslateExercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "MultipleChoiceQuestions");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "MultipleChoiceQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "MultipleChoiceExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "MultipleChoiceExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "MultipleChoiceQuestions");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "MultipleChoiceExercises");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "MultipleChoiceExercises");

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "MultipleChoiceQuestions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
