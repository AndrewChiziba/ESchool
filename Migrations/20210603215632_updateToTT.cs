using Microsoft.EntityFrameworkCore.Migrations;

namespace ESchool.Migrations
{
    public partial class updateToTT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeTableId",
                table: "TTEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TTEntries_TimeTableId",
                table: "TTEntries",
                column: "TimeTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_TTEntries_TimeTables_TimeTableId",
                table: "TTEntries",
                column: "TimeTableId",
                principalTable: "TimeTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TTEntries_TimeTables_TimeTableId",
                table: "TTEntries");

            migrationBuilder.DropIndex(
                name: "IX_TTEntries_TimeTableId",
                table: "TTEntries");

            migrationBuilder.DropColumn(
                name: "TimeTableId",
                table: "TTEntries");
        }
    }
}
