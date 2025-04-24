using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class ChangedWorkerClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Workers_InternId",
                table: "Availabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Workers_PhysiotherapistId",
                table: "Availabilities");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_InternId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "InternId",
                table: "Availabilities");

            migrationBuilder.RenameColumn(
                name: "PhysiotherapistId",
                table: "Availabilities",
                newName: "WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Availabilities_PhysiotherapistId",
                table: "Availabilities",
                newName: "IX_Availabilities_WorkerId");

            migrationBuilder.AlterColumn<int>(
                name: "IntakeSupervisorId",
                table: "PatientRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IntakeHandlerId",
                table: "PatientRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HeadPractitionerId",
                table: "PatientRecords",
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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Workers_WorkerId",
                table: "Availabilities");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Availabilities",
                newName: "PhysiotherapistId");

            migrationBuilder.RenameIndex(
                name: "IX_Availabilities_WorkerId",
                table: "Availabilities",
                newName: "IX_Availabilities_PhysiotherapistId");

            migrationBuilder.AlterColumn<int>(
                name: "IntakeSupervisorId",
                table: "PatientRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IntakeHandlerId",
                table: "PatientRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HeadPractitionerId",
                table: "PatientRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InternId",
                table: "Availabilities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_InternId",
                table: "Availabilities",
                column: "InternId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Workers_InternId",
                table: "Availabilities",
                column: "InternId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Workers_PhysiotherapistId",
                table: "Availabilities",
                column: "PhysiotherapistId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
