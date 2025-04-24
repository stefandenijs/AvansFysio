using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class TestingDeleteBehaviorCascadeOnTreatmentComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentComments_Workers_PlacedById",
                table: "TreatmentComments");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentComments_Workers_PlacedById",
                table: "TreatmentComments",
                column: "PlacedById",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentComments_Workers_PlacedById",
                table: "TreatmentComments");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentComments_Workers_PlacedById",
                table: "TreatmentComments",
                column: "PlacedById",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
