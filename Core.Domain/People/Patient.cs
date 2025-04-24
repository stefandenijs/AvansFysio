using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.People
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public byte[] PatientImage { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public string IdentificationNumber { get; set; }
        
        public PatientRecord.PatientRecord PatientRecord { get; set; }
    }
}
