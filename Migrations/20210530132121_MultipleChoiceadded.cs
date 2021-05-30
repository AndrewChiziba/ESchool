using Microsoft.EntityFrameworkCore.Migrations;

namespace ESchool.Migrations
{
    public partial class MultipleChoiceadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MultipleChoiceExerciseId",
                table: "MultipleChoiceQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MultipleChoiceExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfQuestions = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceExercises", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestions_MultipleChoiceExerciseId",
                table: "MultipleChoiceQuestions",
                column: "MultipleChoiceExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceQuestions_MultipleChoiceExercises_MultipleChoiceExerciseId",
                table: "MultipleChoiceQuestions",
                column: "MultipleChoiceExerciseId",
                principalTable: "MultipleChoiceExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceQuestions_MultipleChoiceExercises_MultipleChoiceExerciseId",
                table: "MultipleChoiceQuestions");

            migrationBuilder.DropTable(
                name: "MultipleChoiceExercises");

            migrationBuilder.DropIndex(
                name: "IX_MultipleChoiceQuestions_MultipleChoiceExerciseId",
                table: "MultipleChoiceQuestions");

            migrationBuilder.DropColumn(
                name: "MultipleChoiceExerciseId",
                table: "MultipleChoiceQuestions");
        }
    }
}
