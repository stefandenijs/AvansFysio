using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class MakingTreatmentsMandatoryInTreatmentComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentComments_Treatments_TreatmentId",
                table: "TreatmentComments");

            migrationBuilder.AlterColumn<int>(
                name: "TreatmentId",
                table: "TreatmentComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentComments_Treatments_TreatmentId",
                table: "TreatmentComments",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentComments_Treatments_TreatmentId",
                table: "TreatmentComments");

            migrationBuilder.AlterColumn<int>(
                name: "TreatmentId",
                table: "TreatmentComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentComments_Treatments_TreatmentId",
                table: "TreatmentComments",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
