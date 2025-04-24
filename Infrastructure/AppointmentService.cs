using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Time;
using Core.DomainServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppointmentService : IAppointmentService
    {
        private readonly PhysioDbContext _context;

        public AppointmentService(PhysioDbContext context)
        {
            _context = context;
        }

        public Appointment GetAppointment(int id)
        {
            var appointment = _context.Appointments
                .Include(a => a.Treatment)
                .SingleOrDefault(a => a.Id == id);
            return appointment;
        }

        public Appointment GetAppointmentByTreatment(int id)
        {
            var appointment = _context.Appointments
                .Include(a => a.Treatment)
                .SingleOrDefault(a => a.Treatment.Id == id);
            return appointment;
        }

        public List<Appointment> GetAllWorkerAppointments(int id)
        {
            var appointments = _context.Appointments.Where(a => a.PractitionerId == id)
                .Include(a => a.Patient)
                .Include(a => a.Practitioner)
                .Include(a => a.Treatment);
            return appointments.ToList();
        }

        public List<Appointment> GetAllWorkerAppointments(DateTime date, int id)
        {
            var appointments =
                _context.Appointments.Where(a => a.StartTime.Date == date.Date && a.PractitionerId == id);
            return appointments.ToList();
        }

        public List<Appointment> GetAllPatientAppointments(int id)
        {
            var appointments = _context.Appointments.Where(a => a.Patient.Id == id)
                .Include(a => a.Patient)
                .Include(a => a.Practitioner)
                .Include(a => a.Treatment);
            return appointments.ToList();
        }

        public List<Appointment> GetAllPatientAppointmentsForWeek(DateTime startTime, int id)
        {
            var days = startTime.DayOfWeek - DayOfWeek.Sunday;
            var startOfWeek = startTime.AddDays(-days);
            var endOfWeek = startOfWeek.AddDays(6);
            var appointments = _context.Appointments.Where(a =>
                a.StartTime >= startOfWeek && a.EndTime <= endOfWeek && a.Patient.Id == id);
            return appointments.ToList();
        }

        public async Task AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Appointment appointment)
        {
            _context.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAppointment(Appointment appointment)
        {
            try
            {
                _context.Remove(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
