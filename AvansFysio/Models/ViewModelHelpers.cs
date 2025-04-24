using System;
using System.Collections.Generic;
using System.Globalization;
using AvansFysio.Models.Appointments;
using AvansFysio.Models.Availability;
using AvansFysio.Models.Comments;
using AvansFysio.Models.Patients;
using AvansFysio.Models.Treatments;
using AvansFysio.Models.Workers;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.People;
using Core.Domain.Time;
using Core.Domain.Treatment;

namespace AvansFysio.Models
{
    public static class ViewModelHelpers
    {
        public static List<PatientsViewModel> ToViewModel(this IEnumerable<Patient> patients)
        {
            var result = new List<PatientsViewModel>();

            foreach (var patient in patients)
            {
                result.Add(patient.ToViewModel());
            }

            return result;
        }

        public static PatientsViewModel ToViewModel(this Patient patient)
        {
            var result = new PatientsViewModel
            {
                PatientId = patient.Id,
                Name = patient.Name,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Gender = patient.Gender,
                Role = patient.Role,
                IdentificationNumber = patient.IdentificationNumber,
                BirthDate = patient.BirthDate
            };

            if (patient.PatientRecord != null)
            {
                result.PatientRecord = patient.PatientRecord.ToViewModel();
            }

            if (patient.PatientImage != null)
            {
                result.ImageSource = "data:image/gif;base64," + Convert.ToBase64String(patient.PatientImage);
            }

            return result;
        }

        public static PatientRecordsViewModel ToViewModel(this PatientRecord patientRecord)
        {
            var result = new PatientRecordsViewModel
            {
                PatientRecordId = patientRecord.Id,
                Age = patientRecord.Age,
                MedicalComplaintsDescription = patientRecord.MedicalComplaintsDescription,
                DiagnosticCode = patientRecord.DiagnosticCode,
                DiagnoseDescription = patientRecord.DiagnoseDescription,
                IntakeHandler = patientRecord.IntakeHandler.Name,
                IntakeSupervisor = "Niet van toepassing.",
                HeadPractitionerId = patientRecord.HeadPractitionerId,
                HeadPractitioner = patientRecord.HeadPractitioner.Name,
                DateOfRegistration = patientRecord.DateOfRegistration,
                DateOfDismissal = patientRecord.DateOfDismissal,
                SessionsPerWeek = patientRecord.TreatmentPlan.SessionsPerWeek,
                SessionsDuration = patientRecord.TreatmentPlan.SessionDuration
            };

            if (patientRecord.IntakeSupervisor != null)
            {
                result.IntakeSupervisor = patientRecord.IntakeSupervisor.Name;
            }

            return result;
        }
        public static List<AvailabilitiesViewModel> ToViewModel(this IEnumerable<Core.Domain.Time.Availability> availabilities)
        {
            var result = new List<AvailabilitiesViewModel>();

            foreach (var availability in availabilities)
            {
                result.Add(availability.ToViewModel());
            }

            return result;
        }

        public static AvailabilitiesViewModel ToViewModel(this Core.Domain.Time.Availability availability)
        {
            var dutch = new CultureInfo("NL-nl");
            var dateTimeInfo = dutch.DateTimeFormat;

            var day = dateTimeInfo.GetDayName(availability.Day);

            var result = new AvailabilitiesViewModel()
            {
                DayOfWeek = char.ToUpper(day[0]) + day[1..],
                Available = availability.Available,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime
            };

            return result;
        }

        public static List<AppointmentsViewModel> ToViewModel(this IEnumerable<Appointment> appointments)
        {
            var result = new List<AppointmentsViewModel>();

            foreach (var appointment in appointments)
            {
                result.Add(appointment.ToViewModel());
            }

            return result;
        }

        public static AppointmentsViewModel ToViewModel(this Appointment appointment)
        {
            var result = new AppointmentsViewModel()
            {
                Id = appointment.Id,
                Practitioner = appointment.Practitioner.Name,
                PatientId = appointment.PatientId,
                Patient = appointment.Patient.Name,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime
            };

            if (appointment.Treatment != null)
            {
                result.Treatment = $"{appointment.Treatment.Description}";
            }

            return result;
        }
        public static List<TreatmentsViewModel> ToViewModel(this IEnumerable<Treatment> treatments)
        {
            var result = new List<TreatmentsViewModel>();

            foreach (var treatment in treatments)
            {
                result.Add(treatment.ToViewModel());
            }

            return result;
        }

        public static TreatmentsViewModel ToViewModel(this Treatment treatment)
        { var result = new TreatmentsViewModel()
            {
                Id = treatment.Id,
                PatientId = treatment.PatientRecord.PatientId,
                Patient = treatment.PatientRecord.Patient.Name,
                Code = treatment.Code,
                Practitioner = treatment.TreatmentPerformedBy.Name,
                Description = treatment.Description,
                Particularities = "Geen bijzonderheden.",
                Room = $"{treatment.Room.RoomType} {treatment.Room.RoomNumber}",
                PerformedOn = treatment.TreatmentPerformedDate,
                CreatedOn = treatment.CreationDate
            };

            if (treatment.Particularities != null)
            {
                result.Particularities = treatment.Particularities;
            }

            return result;
        }

        public static List<CommentsViewModel> ToViewModel(this IEnumerable<Comment> comments)
        {
            var result = new List<CommentsViewModel>();

            foreach (var comment in comments)
            {
                result.Add(comment.ToViewModel());
            }

            return result;
        }

        public static CommentsViewModel ToViewModel(this Comment comment)
        {
            var result = new CommentsViewModel()
            {
                CommentText = comment.CommentText,
                Visible = comment.Visible,
                PlacedBy = comment.PlacedBy.Name,
                Date = comment.Date
            };

            return result;
        }

        public static WorkersViewModel ToViewModel(this Worker worker)
        {
            var result = new WorkersViewModel
            {
                Id = worker.Id,
                Name = worker.Name,
                Email = worker.Email
            };
            return result;
        }
        public static PhysiotherapistsViewModel ToViewModel(this Physiotherapist physiotherapist)
        {
            var result = new PhysiotherapistsViewModel
            {
                Id = physiotherapist.Id,
                Name = physiotherapist.Name,
                Email = physiotherapist.Email,
                PhoneNumber = physiotherapist.PhoneNumber,
                IdentificationNumber = physiotherapist.EmployeeNumber,
                BigNumber = physiotherapist.BigNumber
            };
            return result;
        }
        public static InternsViewModel ToViewModel(this Intern intern)
        {
            var result = new InternsViewModel
            {
                Id = intern.Id,
                Name = intern.Name,
                Email = intern.Email,
                IdentificationNumber = intern.StudentNumber
            };
            return result;
        }
    }
}
