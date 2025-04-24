using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Time;

namespace Core.DomainServices
{
    public interface IAppointmentService
    {
        public Appointment GetAppointment(int id);
        public Appointment GetAppointmentByTreatment(int id);
        public List<Appointment> GetAllWorkerAppointments(int id);
        public List<Appointment> GetAllWorkerAppointments(DateTime date, int id);
        public List<Appointment> GetAllPatientAppointments(int id);
        public List<Appointment> GetAllPatientAppointmentsForWeek(DateTime date, int id);
        public Task AddAppointment(Appointment appointment);
        public Task UpdateAppointment(Appointment appointment);
        public Task<bool> DeleteAppointment(Appointment appointment);
    }
}
