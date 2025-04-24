using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddingTreatmentsToAppointments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BigNumber", "EmployeeNumber" },
                values: new object[] { "25632", "302" });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "Id", "Discriminator", "Email", "Name", "StudentNumber" },
                values: new object[,]
                {
                    { 3, "Intern", "bam.vheng@student.avans.nl", "Bidgret van Hengelo", "243233" },
                    { 4, "Intern", "r.vrij@student.avans.nl", "Rens van Rijen", "243372" },
                    { 5, "Intern", "m.rodriguez@student.avans.nl", "Magdela Rodriguez", "242331" }
                });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "Id", "BigNumber", "Discriminator", "Email", "EmployeeNumber", "Name", "PhoneNumber" },
                values: new object[] { 2, "27042", "Physiotherapist", "m.frueger@docent.avans.nl", "326", "Mark Frueger", "0664326365" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TreatmentId",
                table: "Appointments",
                column: "TreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Treatments_TreatmentId",
                table: "Appointments",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Treatments_TreatmentId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_TreatmentId",
                table: "Appointments");

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "TreatmentId",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BigNumber", "EmployeeNumber" },
                values: new object[] { "12345678911", "432432" });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "Id", "Discriminator", "Email", "Name", "StudentNumber" },
                values: new object[] { 2, "Intern", "bam.vheng@student.avans.nl", "Bidgret van Hengelo", "243233" });
        }
    }
}
