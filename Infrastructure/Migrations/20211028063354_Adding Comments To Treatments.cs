using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddingCommentsToTreatments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TreatmentComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlacedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentComments_Workers_PlacedById",
                        column: x => x.PlacedById,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TreatmentId",
                table: "Comments",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentComments_PlacedById",
                table: "TreatmentComments",
                column: "PlacedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Treatments_TreatmentId",
                table: "Comments",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Treatments_TreatmentId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "TreatmentComments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TreatmentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "Comments");
        }
    }
}
