using System;

namespace Core.DomainServices.Utility
{
    public class AppointmentPlanner : IAppointmentPlanner
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAvailabilityService _availabilityService;

        public AppointmentPlanner(IAppointmentService appointmentService, IAvailabilityService availabilityService)
        {
            _appointmentService = appointmentService;
            _availabilityService = availabilityService;
        }

        public bool CheckForAmountOfAppointments(int patientId, DateTime dateTime, int maxAppointmentsPerWeek)
        {
            var appointmentCount = _appointmentService.GetAllPatientAppointmentsForWeek(dateTime, patientId).Count;
            return appointmentCount + 1 <= maxAppointmentsPerWeek;
        }

        public bool CheckForAmountOfAppointmentsUpdate(int patientId, DateTime dateTime, int maxAppointmentsPerWeek)
        {
            var appointmentCount = _appointmentService.GetAllPatientAppointmentsForWeek(dateTime, patientId).Count;
            return appointmentCount <= maxAppointmentsPerWeek;
        }


        public bool CheckAvailability(int workerId, DateTime startTime, DateTime endTime)
        {
            var availability = _availabilityService.GetAvailability(workerId,
                startTime.DayOfWeek);

            if (availability == null || !availability.Available)
            {
                return false;
            }

            return (startTime.TimeOfDay >= availability.StartTime.Value.TimeOfDay 
                    && endTime.TimeOfDay >= availability.StartTime.Value.TimeOfDay 
                    && startTime.TimeOfDay <= availability.EndTime.Value.TimeOfDay 
                    && endTime.TimeOfDay <= availability.EndTime.Value.TimeOfDay);
        }

        public bool CheckForAppointments(int workerId, DateTime startTime, DateTime endTime)
        {
            var appointmentsOnDay =
                _appointmentService.GetAllWorkerAppointments(startTime.Date, workerId);
            foreach (var appointment in appointmentsOnDay)
            {
                if ((startTime > appointment.StartTime &&
                     startTime < appointment.EndTime) ||
                    (endTime > appointment.StartTime &&
                     endTime < appointment.EndTime))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
