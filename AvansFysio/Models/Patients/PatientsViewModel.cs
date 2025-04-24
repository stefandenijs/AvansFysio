using System;
using Core.Domain.People;
using Microsoft.AspNetCore.Http;

namespace AvansFysio.Models.Patients
{
    public class PatientsViewModel
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string ImageSource { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }
        public string IdentificationNumber { get; set; }
        public PatientRecordsViewModel PatientRecord { get; set; }
    }
}
