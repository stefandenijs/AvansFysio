using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class SmallChangeToSeeIfWorkerIdGetsAddedToTheAvailabilityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Workers_WorkerId",
                table: "Availabilities");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerId",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Workers_WorkerId",
                table: "Availabilities",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Workers_WorkerId",
                table: "Availabilities");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerId",
                table: "Availabilities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Workers_WorkerId",
                table: "Availabilities",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
