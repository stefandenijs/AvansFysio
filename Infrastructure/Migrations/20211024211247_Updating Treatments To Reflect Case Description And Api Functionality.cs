using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdatingTreatmentsToReflectCaseDescriptionAndApiFunctionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Treatments",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "Room",
                table: "Treatments",
                newName: "Particularities");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Treatments",
                newName: "Code");

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_RoomId",
                table: "Treatments",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_Room_RoomId",
                table: "Treatments",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_Room_RoomId",
                table: "Treatments");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_RoomId",
                table: "Treatments");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Treatments",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Particularities",
                table: "Treatments",
                newName: "Room");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Treatments",
                newName: "Comments");
        }
    }
}
