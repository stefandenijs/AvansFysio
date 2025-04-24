using System;

namespace Core.DomainServices.Utility
{
    public interface IAppointmentPlanner
    {
        public bool CheckForAmountOfAppointments(int patientId, DateTime dateTime, int maxAppointmentsPerWeek);
        public bool CheckForAmountOfAppointmentsUpdate(int patientId, DateTime dateTime, int maxAppointmentsPerWeek);
        public bool CheckAvailability(int workerId, DateTime startTime, DateTime endTime);
        public bool CheckForAppointments(int workerId, DateTime startTime, DateTime endTime);
    }
}
