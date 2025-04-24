using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AvansFysio.Controllers;
using AvansFysio.Models;
using AvansFysio.Models.Patients;
using Core.Domain.People;
using Core.DomainServices;
using Core.DomainServices.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AvansFysio.tests
{
    public class PatientControllerTests
    {
        [Fact]
        public async void Age_Must_Be_16_Or_Older_Successfully_Added()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PatientController>>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var ageHelperMock = new Mock<IAgeHelper>();
            var diagnosisServiceMock = new Mock<IDiagnosisServiceHttp>();
            var sut = new PatientController(loggerMock.Object, patientServiceMock.Object, workerServiceMock.Object, ageHelperMock.Object, diagnosisServiceMock.Object);
            ageHelperMock.Setup(ageHelperMock => ageHelperMock.CheckAge(It.IsAny<DateTime>())).Returns(true);
            patientServiceMock.Setup(patientService => patientService.GetPatients()).Returns(SeedPatients());
            patientServiceMock.Setup(patientService => patientService.AddPatient(It.IsAny<Patient>())).Verifiable();
            patientServiceMock.Setup(patientService => patientService.GetPatientByEmail(It.IsAny<string>())).Returns(new Patient
            {
                Id = 4,
                Name = "Derick Jansen",
                PhoneNumber = "0623232323",
                Email = "Derick@Jansen.nl",
                Gender = Gender.Man,
                BirthDate = new DateTime(1996, 5, 12),
                PatientImage = Array.Empty<byte>(),
                Role = Role.Student,
                IdentificationNumber = "2042242"
            });
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 0, "Data", "image.png");

            // Act
            var newPatientModel = new NewPatientViewModel
            {
                Name = "Derick Jansen",
                PhoneNumber = "0623232323",
                Email = "Derick@Jansen.nl",
                Gender = Gender.Man,
                BirthDate = new DateTime(1996, 5, 12),
                Photo = file,
                Role = Role.Student,
                IdentificationNumber = "2042242"
            };

            var result = await sut.PatientForm(newPatientModel) as RedirectToActionResult;

            // Assert
            patientServiceMock.Verify(patientService => patientService.AddPatient(It.IsAny<Patient>()), Times.Once());
        }

        [Fact]
        public async void Age_Must_Be_16_Or_Older_Failure_With_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PatientController>>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var ageHelperMock = new Mock<IAgeHelper>();
            var diagnosisServiceMock = new Mock<IDiagnosisServiceHttp>();
            var sut = new PatientController(loggerMock.Object, patientServiceMock.Object, workerServiceMock.Object, ageHelperMock.Object, diagnosisServiceMock.Object);
            ageHelperMock.Setup(ageHelperMock => ageHelperMock.CheckAge(It.IsAny<DateTime>())).Returns(false);
            patientServiceMock.Setup(patientService => patientService.GetPatients()).Returns(SeedPatients());
            patientServiceMock.Setup(patientService => patientService.AddPatient(It.IsAny<Patient>())).Verifiable();
            patientServiceMock.Setup(patientService => patientService.GetPatientByEmail(It.IsAny<string>())).Returns(new Patient
            {
                Id = 4,
                Name = "Derick Jansen",
                PhoneNumber = "0623232323",
                Email = "Derick@Jansen.nl",
                Gender = Gender.Man,
                BirthDate = new DateTime(1996, 5, 12),
                PatientImage = Array.Empty<byte>(),
                Role = Role.Student,
                IdentificationNumber = "2042242"
            });
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 0, "Data", "image.png");

            // Act
            var newPatientModel = new NewPatientViewModel
            {
                Name = "Derick Jansen",
                PhoneNumber = "0623232323",
                Email = "Derick@Jansen.nl",
                Gender = Gender.Man,
                BirthDate = new DateTime(2008, 5, 12),
                Photo = file,
                Role = Role.Student,
                IdentificationNumber = "2042242"
            };

            var result = await sut.PatientForm(newPatientModel) as RedirectToActionResult;

            // Assert
            patientServiceMock.Verify(patientService => patientService.AddPatient(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public void Patients_Should_Return_Patients_In_Model()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<PatientController>>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var ageHelperMock = new Mock<IAgeHelper>();
            var diagnosisServiceMock = new Mock<IDiagnosisServiceHttp>();
            var sut = new PatientController(loggerMock.Object, patientServiceMock.Object, workerServiceMock.Object, ageHelperMock.Object, diagnosisServiceMock.Object);
            patientServiceMock.Setup(patientService => patientService.GetPatients()).Returns(SeedPatients());

            // Act
            var result = sut.Patients() as ViewResult;

            // Assert
            var patientsInModel = result.Model as List<PatientsViewModel>;

            Assert.Equal(3, patientsInModel.Count);
            Assert.Equal("Frank de Boer",
                patientsInModel[0].Name);
            Assert.Equal("Melissa de Jonge",
                patientsInModel[1].Name);
        }

        [Fact]
        public void Details_Should_Return_Single_Patient_In_Model()
        {
            // Arrange
            var patients = SeedPatients();
            var patientToTest = patients[1];
            var loggerMock = new Mock<ILogger<PatientController>>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var ageHelperMock = new Mock<IAgeHelper>();
            var diagnosisServiceMock = new Mock<IDiagnosisServiceHttp>();
            var sut = new PatientController(loggerMock.Object, patientServiceMock.Object, workerServiceMock.Object, ageHelperMock.Object, diagnosisServiceMock.Object);
            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(patientToTest);

            // Act
            var result = sut.Details(2) as ViewResult;

            // Assert
            var patientInModel = result.Model as PatientsViewModel;

            Assert.True(patientInModel != null);
            patientInModel.Equals(patientToTest.ToViewModel());
        }

        public List<Patient> SeedPatients()
        {
            return new List<Patient>
            {
                new Patient {Id = 1, Name = "Frank de Boer", Email =  "frank@boer.nl", PhoneNumber = "0643434344", Gender = Gender.Man, BirthDate = new DateTime(1978, 6, 20)},
                new Patient {Id = 2, Name = "Melissa de Jonge", Email =  "Mel@Jonge.com",PhoneNumber = "0643434344", Gender = Gender.Vrouw, BirthDate = new DateTime(1996, 3, 12)},
                new Patient {Id = 3, Name = "Mathieu Demontreux", Email = "mtdemont@orange.fr",PhoneNumber = "0643434344", Gender = Gender.Man, BirthDate = new DateTime(1999, 2, 3)}
            };
        }

        public Patient GetPatient()
        {
            return new Patient()
            {
                Id = 3,
                Name = "Mathieu Demontreux",
                Email = "mtdemont@orange.fr",
                PhoneNumber = "0643434344",
                Gender = Gender.Man,
                BirthDate = new DateTime(1999, 2, 3)
            };
        }
    }
}
