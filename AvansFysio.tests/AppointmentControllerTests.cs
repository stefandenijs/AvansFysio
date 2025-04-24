using System;
using System.Collections.Generic;
using AvansFysio.Controllers;
using AvansFysio.Models.Appointments;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.People;
using Core.Domain.Time;
using Core.Domain.Treatment;
using Core.DomainServices;
using Core.DomainServices.Utility;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AvansFysio.tests
{
    public class AppointmentControllerTests
    {
        [Fact]
        public void Add_Appointment_Worker_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentModel = new NewAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00),
            };

            var result = sut.AppointmentWorkerForm(newAppointmentModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void Update_Appointment_Worker_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointment());
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var updateAppointmentsViewModel = new UpdateAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 8, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 8, 14, 30, 00)
            };

            var result = sut.UpdateAppointmentWorkerForm(updateAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void Add_Appointment_Patient_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentModel = new NewAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 8, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 8, 14, 30, 00)
            };

            var result = sut.AppointmentPatientForm(newAppointmentModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void Update_Appointment_Patient_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointment());
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var updateAppointmentsViewModel = new UpdateAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 8, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 8, 14, 30, 00)
            };

            var result = sut.UpdateAppointmentPatientForm(updateAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void Maximum_Amount_Of_Appointments_Worker_Per_Week_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentModel = new NewAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 8, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 8, 14, 30, 00)
            };

            var result = sut.AppointmentWorkerForm(newAppointmentModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Maximum_Amount_Of_Appointments_Worker_On_Update_Per_Week_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointmentToday());
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var updateAppointmentsViewModel = new UpdateAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = DateTime.Now.AddDays(-1),
                EndTime = DateTime.Now.AddMinutes(30).AddDays(-1)
            };

            var result = sut.UpdateAppointmentWorkerForm(updateAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Maximum_Amount_Of_Appointments_Patient_Per_Week_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentModel = new NewAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00)
            };

            var result = sut.AppointmentPatientForm(newAppointmentModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Maximum_Amount_Of_Appointments_Patient_On_Update_Per_Week_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointmentToday());
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var updateAppointmentsViewModel = new UpdateAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00)
            };

            var result = sut.UpdateAppointmentPatientForm(updateAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Availability_Appointments_Worker_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentModel = new NewAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00),
            };

            var result = sut.AppointmentWorkerForm(newAppointmentModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Availability_Appointments_Patient_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);
            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentModel = new NewAppointmentsViewModel
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00),
            };

            var result = sut.AppointmentPatientForm(newAppointmentModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Availability_Appointments_Worker_Update_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointment());
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var updateAppointmentsViewModel = new UpdateAppointmentsViewModel()
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00),
            };

            var result = sut.UpdateAppointmentWorkerForm(updateAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Availability_Appointments_Patient_Update_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[2]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointment());
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var updateAppointmentsViewModel = new UpdateAppointmentsViewModel()
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00),
            };

            var result = sut.UpdateAppointmentWorkerForm(updateAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.UpdateAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Appointments_Worker_Form_Patient_Not_Registered_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[1]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentsViewModel = new NewAppointmentsViewModel()
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00),
            };

            var result = sut.AppointmentWorkerForm(newAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Appointments_Patient_Form_Patient_Not_Registered_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            patientServiceMock.Setup(patientService => patientService.GetPatientById(It.IsAny<int>())).Returns(GetPatients()[1]);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAmountOfAppointmentsUpdate(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<int>()))
                .Returns(true);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckAvailability(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(false);
            appointmentPlannerMock.Setup(appointmentPlanner =>
                    appointmentPlanner.CheckForAppointments(It.IsAny<int>(), It.IsAny<DateTime>(),
                        It.IsAny<DateTime>()))
                .Returns(true);
            appointmentServiceMock
                .Setup(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()))
                .Verifiable();

            // Act
            var newAppointmentsViewModel = new NewAppointmentsViewModel()
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 5, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 5, 14, 30, 00),
            };

            var result = sut.AppointmentPatientForm(newAppointmentsViewModel);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.AddAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public void Cancel_Appointment_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointment());

            // Act
            var result = sut.CancelAppointment(1);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.DeleteAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public void Cancel_Appointment_Error()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<AppointmentController>>();
            var appointmentServiceMock = new Mock<IAppointmentService>();
            var patientServiceMock = new Mock<IPatientService>();
            var workerServiceMock = new Mock<IWorkerService>();
            var availabilityServiceMock = new Mock<IAvailabilityService>();
            var appointmentPlannerMock = new Mock<IAppointmentPlanner>();
            var treatmentServiceMock = new Mock<ITreatmentService>();
            var sut = new AppointmentController(loggerMock.Object, appointmentServiceMock.Object,
                patientServiceMock.Object, workerServiceMock.Object, availabilityServiceMock.Object,
                appointmentPlannerMock.Object, treatmentServiceMock.Object);

            appointmentServiceMock.Setup(appointmentService => appointmentService.GetAppointment(It.IsAny<int>()))
                .Returns(GetAppointmentToday());

            // Act
            var result = sut.CancelAppointment(1);

            // Assert
            appointmentServiceMock.Verify(appointmentService => appointmentService.DeleteAppointment(It.IsAny<Appointment>()), Times.Never);
        }

        private Appointment GetAppointmentToday()
        {
            return new Appointment
            {
                PatientId = 3,
                PractitionerId = 1,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMinutes(30)
            };
        }

        private Appointment GetAppointment()
        {
            return new Appointment
            {
                Id = 1,
                PatientId = 3,
                PractitionerId = 1,
                StartTime = new DateTime(2021, 11, 7, 14, 00, 00),
                EndTime = new DateTime(2021, 11, 7, 14, 30, 00)
            };
        }

        private List<Worker> GetWorkers()
        {
            return new List<Worker>
            {
                new()
                {
                    Id = 1,
                    Name = "Freek de Boer",
                    Email = "tests@worker.nl"
                },
                new()
                {
                    Id = 2,
                    Name = "Bidgret van Hengelo",
                    Email = "bam.vheng@student.avans.nl"
                }
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
                        }
                    }
                }
            };
        }
    }
}
