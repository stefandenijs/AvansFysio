using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddingPatientRecordIdToTreamentClassSoIcanActuallyCallIt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_PatientRecords_PatientRecordId",
                table: "Treatments");

            migrationBuilder.AlterColumn<int>(
                name: "PatientRecordId",
                table: "Treatments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_PatientRecords_PatientRecordId",
                table: "Treatments",
                column: "PatientRecordId",
                principalTable: "PatientRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_PatientRecords_PatientRecordId",
                table: "Treatments");

            migrationBuilder.AlterColumn<int>(
                name: "PatientRecordId",
                table: "Treatments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_PatientRecords_PatientRecordId",
                table: "Treatments",
                column: "PatientRecordId",
                principalTable: "PatientRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
