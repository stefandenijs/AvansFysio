using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddingCorrectCommentsToTreatments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Treatments_TreatmentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TreatmentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "TreatmentComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentComments_TreatmentId",
                table: "TreatmentComments",
                column: "TreatmentId");

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

            migrationBuilder.DropIndex(
                name: "IX_TreatmentComments_TreatmentId",
                table: "TreatmentComments");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "TreatmentComments");

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TreatmentId",
                table: "Comments",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Treatments_TreatmentId",
                table: "Comments",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
