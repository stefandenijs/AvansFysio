using System;
using System.Collections.Generic;
using AvansFysio.Controllers;
using AvansFysio.Models.Treatments;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.People;
using Core.Domain.Treatment;
using Core.Domain.Webservice;
using Core.DomainServices;
using Moq;
using Xunit;

namespace AvansFysio.tests
{
    public class TreatmentControllerTests
    {
        [Fact]
        public void Treatment_Added_Success()
        {
            // Arrange
            var treatmentInfoServiceHttpMock = new Mock<ITreatmentInfoServiceHttp>();
            var roomServiceMock = new Mock<IRoomService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var sut = new TreatmentController(treatmentInfoServiceHttpMock.Object, roomServiceMock.Object,
                workerServiceMock.Object, treatmentServiceMock.Object, patientServiceMock.Object, appointmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            treatmentInfoServiceHttpMock.Setup(treatmentInfoServiceHttp =>
                treatmentInfoServiceHttp.GetTreatmentInfo(It.IsAny<int>())).Returns(GetTreatmentInfo());

            //Act
            var newTreatmentViewModel = new NewTreatmentsViewModel
            {
                PatientId = 3,
                Particularities = "Particularities",
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 10, 20)
            };

            var result = sut.TreatmentForm(newTreatmentViewModel);

            //Assert
            patientServiceMock.Verify(patientService => patientService.UpdatePatient(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public void Treatment_Updated_Success()
        {
            // Arrange
            var treatmentInfoServiceHttpMock = new Mock<ITreatmentInfoServiceHttp>();
            var roomServiceMock = new Mock<IRoomService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var sut = new TreatmentController(treatmentInfoServiceHttpMock.Object, roomServiceMock.Object,
                workerServiceMock.Object, treatmentServiceMock.Object, patientServiceMock.Object, appointmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            treatmentInfoServiceHttpMock.Setup(treatmentInfoServiceHttp =>
                treatmentInfoServiceHttp.GetTreatmentInfo(It.IsAny<int>())).Returns(GetTreatmentInfo());
            treatmentServiceMock.Setup(treatmentService =>
                treatmentService.GetTreatment(It.IsAny<int>())).Returns(GetTreatmentForUpdate());

            //Act
            var updateTreatmentsViewModel = new UpdateTreatmentsViewModel()
            {
                PatientId = 3,
                Particularities = "Particularities",
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 10, 20)
            };

            var result = sut.UpdateTreatmentForm(updateTreatmentsViewModel);

            //Assert
            treatmentServiceMock.Verify(treatmentService => treatmentService.UpdateTreatment(It.IsAny<Treatment>()), Times.Once);
        }

        [Fact]
        public void Treatment_Added_Patient_Not_Registered_Error()
        {
            // Arrange
            var treatmentInfoServiceHttpMock = new Mock<ITreatmentInfoServiceHttp>();
            var roomServiceMock = new Mock<IRoomService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var sut = new TreatmentController(treatmentInfoServiceHttpMock.Object, roomServiceMock.Object,
                workerServiceMock.Object, treatmentServiceMock.Object, patientServiceMock.Object, appointmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[1]);
            treatmentInfoServiceHttpMock.Setup(treatmentInfoServiceHttp =>
                treatmentInfoServiceHttp.GetTreatmentInfo(It.IsAny<int>())).Returns(GetTreatmentInfo());

            //Act
            var newTreatmentViewModel = new NewTreatmentsViewModel
            {
                PatientId = 3,
                Particularities = "Particularities",
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 11, 30)
            };

            var result = sut.TreatmentForm(newTreatmentViewModel);

            //Assert
            patientServiceMock.Verify(patientService => patientService.UpdatePatient(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void Treatment_Added_Outside_Of_Patient_Treatment_Period_Error()
        {
            // Arrange
            var treatmentInfoServiceHttpMock = new Mock<ITreatmentInfoServiceHttp>();
            var roomServiceMock = new Mock<IRoomService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var sut = new TreatmentController(treatmentInfoServiceHttpMock.Object, roomServiceMock.Object,
                workerServiceMock.Object, treatmentServiceMock.Object, patientServiceMock.Object, appointmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            treatmentInfoServiceHttpMock.Setup(treatmentInfoServiceHttp =>
                treatmentInfoServiceHttp.GetTreatmentInfo(It.IsAny<int>())).Returns(GetTreatmentInfo());

            //Act
            var newTreatmentViewModel = new NewTreatmentsViewModel
            {
                PatientId = 3,
                Particularities = "Particularities",
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 11, 30)
            };

            var result = sut.TreatmentForm(newTreatmentViewModel);

            //Assert
            patientServiceMock.Verify(patientService => patientService.UpdatePatient(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void Treatment_Updated_Outside_Of_Patient_Treatment_Period_Error()
        {
            // Arrange
            var treatmentInfoServiceHttpMock = new Mock<ITreatmentInfoServiceHttp>();
            var roomServiceMock = new Mock<IRoomService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var sut = new TreatmentController(treatmentInfoServiceHttpMock.Object, roomServiceMock.Object,
                workerServiceMock.Object, treatmentServiceMock.Object, patientServiceMock.Object, appointmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            treatmentInfoServiceHttpMock.Setup(treatmentInfoServiceHttp =>
                treatmentInfoServiceHttp.GetTreatmentInfo(It.IsAny<int>())).Returns(GetTreatmentInfo());
            treatmentServiceMock.Setup(treatmentService =>
                treatmentService.GetTreatment(It.IsAny<int>())).Returns(GetTreatmentForUpdate());

            //Act
            var updateTreatmentsViewModel = new UpdateTreatmentsViewModel()
            {
                PatientId = 3,
                Particularities = "Particularities",
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 11, 30)
            };

            var result = sut.UpdateTreatmentForm(updateTreatmentsViewModel);

            //Assert
            treatmentServiceMock.Verify(treatmentService => treatmentService.UpdateTreatment(It.IsAny<Treatment>()), Times.Never);
        }

        [Fact]
        public void Treatment_Added_Missing_Particularities_When_Required_Error()
        {
            // Arrange
            var treatmentInfoServiceHttpMock = new Mock<ITreatmentInfoServiceHttp>();
            var roomServiceMock = new Mock<IRoomService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var sut = new TreatmentController(treatmentInfoServiceHttpMock.Object, roomServiceMock.Object,
                workerServiceMock.Object, treatmentServiceMock.Object, patientServiceMock.Object, appointmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            treatmentInfoServiceHttpMock.Setup(treatmentInfoServiceHttp =>
                treatmentInfoServiceHttp.GetTreatmentInfo(It.IsAny<int>())).Returns(GetTreatmentInfo());

            //Act
            var newTreatmentViewModel = new NewTreatmentsViewModel
            {
                PatientId = 3,
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 10, 20)
            };

            var result = sut.TreatmentForm(newTreatmentViewModel);

            //Assert
            patientServiceMock.Verify(patientServiceMock => patientServiceMock.UpdatePatient(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void Treatment_Updated_Missing_Particularities_When_Required_Error()
        {
            // Arrange
            var treatmentInfoServiceHttpMock = new Mock<ITreatmentInfoServiceHttp>();
            var roomServiceMock = new Mock<IRoomService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var sut = new TreatmentController(treatmentInfoServiceHttpMock.Object, roomServiceMock.Object,
                workerServiceMock.Object, treatmentServiceMock.Object, patientServiceMock.Object, appointmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            treatmentInfoServiceHttpMock.Setup(treatmentInfoServiceHttp =>
                treatmentInfoServiceHttp.GetTreatmentInfo(It.IsAny<int>())).Returns(GetTreatmentInfo());
            treatmentServiceMock.Setup(treatmentService =>
                treatmentService.GetTreatment(It.IsAny<int>())).Returns(GetTreatmentForUpdate());

            //Act
            var updateTreatmentsViewModel = new UpdateTreatmentsViewModel()
            {
                PatientId = 3,
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 10, 20)
            };

            var result = sut.UpdateTreatmentForm(updateTreatmentsViewModel);

            //Assert
            treatmentServiceMock.Verify(treatmentService => treatmentService.UpdateTreatment(It.IsAny<Treatment>()), Times.Never);
        }

        private Treatment GetTreatment()
        {
            return new Treatment
            {
                Code = "1000",
                Description = "Treatment info",
                CreationDate = new DateTime(2021, 10, 19),
                Particularities = "Particularities",
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = new DateTime(2021, 10, 19)
            };
        }

        private Treatment GetTreatmentForUpdate()
        {
            return new Treatment
            {
                Code = "1000",
                Description = "Treatment info",
                CreationDate = DateTime.Now,
                Particularities = "Particularities",
                RoomId = 4,
                TreatmentInfoId = 200,
                TreatmentPerformedById = 1,
                TreatmentPerformedDate = DateTime.Now
            };
        }

        private TreatmentInfo GetTreatmentInfo()
        {
            return new TreatmentInfo
            {
                Code = "1000",
                Description = "Treatment info",
                ExplanationRequired = "Ja"
            };
        }

        private List<Patient> GetPatients()
        {
            return new List<Patient> {
                new()
                {

                    Id = 1,
                    Name = "Frank de Boer",
                    Email = "frank@boer.nl",
                    PhoneNumber = "0643434344",
                    Gender = Gender.Man,
                    BirthDate = new DateTime(1978, 6, 20),
                    Role = Role.Docent,
                    IdentificationNumber = "425"
                },
                new()
                {
                    Id = 2,
                    Name = "Melissa de Jonge",
                    Email = "Mel@Jonge.com",
                    PhoneNumber = "0643923744",
                    Gender = Gender.Vrouw,
                    BirthDate = new DateTime(1996, 3, 12),
                    Role = Role.Student,
                    IdentificationNumber = "2174234"
                },
                new()
                {
                    Id = 3,
                    Name = "Mathieu Demontreux",
                    Email = "mtdemont@orange.fr",
                    PhoneNumber = "0674054252",
                    Gender = Gender.Man,
                    BirthDate = new DateTime(1999, 2, 3),
                    Role = Role.Student,
                    IdentificationNumber = "2125345",
                    PatientRecord = new PatientRecord
                    {
                        Age = 24,
                        Comments = null,
                        DateOfRegistration = new DateTime(2021, 10, 10),
                        DateOfDismissal = new DateTime(2021, 11, 20),
                        DiagnoseId = 1,
                        DiagnosticBodyLocalization = "Test locatie",
                        DiagnosticCode = "1000",
                        DiagnoseDescription = "Een test diagnose",
                        HeadPractitionerId = 1,
                        TreatmentPlan = new TreatmentPlan
                        {
                            Id = 3,
                            SessionsPerWeek = 3,
                            SessionDuration = 30
                        },
                        Treatments = new List<Treatment>()
                    }
                }
            };
        }
    }
}
