using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionsPerWeek = table.Column<int>(type: "int", nullable: false),
                    SessionDuration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentNumber = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: true),
                    BigNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    TimeMin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeMax = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternId = table.Column<int>(type: "int", nullable: true),
                    PhysiotherapistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Availabilities_Workers_InternId",
                        column: x => x.InternId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Availabilities_Workers_PhysiotherapistId",
                        column: x => x.PhysiotherapistId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    MedicalComplaintsDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiagnosticCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiagnoseDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntakeHandlerId = table.Column<int>(type: "int", nullable: true),
                    IntakeSupervisorId = table.Column<int>(type: "int", nullable: true),
                    HeadPractitionerId = table.Column<int>(type: "int", nullable: true),
                    DateOfRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfDismissal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TreatmentPlanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientRecords_TreatmentPlans_TreatmentPlanId",
                        column: x => x.TreatmentPlanId,
                        principalTable: "TreatmentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientRecords_Workers_HeadPractitionerId",
                        column: x => x.HeadPractitionerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientRecords_Workers_IntakeHandlerId",
                        column: x => x.IntakeHandlerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientRecords_Workers_IntakeSupervisorId",
                        column: x => x.IntakeSupervisorId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    PlacedById = table.Column<int>(type: "int", nullable: false),
                    PatientRecordId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_PatientRecords_PatientRecordId",
                        column: x => x.PatientRecordId,
                        principalTable: "PatientRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Workers_PlacedById",
                        column: x => x.PlacedById,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentPerformedById = table.Column<int>(type: "int", nullable: false),
                    TreatmentPerformedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientRecordId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treatments_PatientRecords_PatientRecordId",
                        column: x => x.PatientRecordId,
                        principalTable: "PatientRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Treatments_Workers_TreatmentPerformedById",
                        column: x => x.TreatmentPerformedById,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentId = table.Column<int>(type: "int", nullable: true),
                    PractitionerId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Treatments_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Workers_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "BirthDate", "Email", "Gender", "IdentificationNumber", "Name", "PatientImage", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(1978, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "frank@boer.nl", 0, "425", "Frank de Boer", null, "0643434344", 0 },
                    { 2, new DateTime(1996, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mel@Jonge.com", 1, "2174234", "Melissa de Jonge", null, "0643923744", 1 },
                    { 3, new DateTime(1999, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "mtdemont@orange.fr", 0, "2125345", "Mathieu Demontreux", null, "0674054252", 1 }
                });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "Id", "Discriminator", "Email", "Name", "StudentNumber" },
                values: new object[] { 2, "Intern", "bam.vheng@student.avans.nl", "Bidgret van Hengelo", 243233 });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "Id", "BigNumber", "Discriminator", "Email", "EmployeeNumber", "Name", "PhoneNumber" },
                values: new object[] { 1, "12345678911", "Physiotherapist", "d.m@docent.avans.nl", 432432, "Daniel Marssen", "0653242635" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PractitionerId",
                table: "Appointments",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TreatmentId",
                table: "Appointments",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_InternId",
                table: "Availabilities",
                column: "InternId");

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_PhysiotherapistId",
                table: "Availabilities",
                column: "PhysiotherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PatientRecordId",
                table: "Comments",
                column: "PatientRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PlacedById",
                table: "Comments",
                column: "PlacedById");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_HeadPractitionerId",
                table: "PatientRecords",
                column: "HeadPractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_IntakeHandlerId",
                table: "PatientRecords",
                column: "IntakeHandlerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_IntakeSupervisorId",
                table: "PatientRecords",
                column: "IntakeSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_PatientId",
                table: "PatientRecords",
                column: "PatientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientRecords_TreatmentPlanId",
                table: "PatientRecords",
                column: "TreatmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientRecordId",
                table: "Treatments",
                column: "PatientRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_TreatmentPerformedById",
                table: "Treatments",
                column: "TreatmentPerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Email",
                table: "Workers",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "PatientRecords");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "TreatmentPlans");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
