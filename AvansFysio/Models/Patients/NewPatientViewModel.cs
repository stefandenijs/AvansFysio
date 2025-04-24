using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain.People;
using Microsoft.AspNetCore.Http;

namespace AvansFysio.Models.Patients
{
    public class NewPatientViewModel
    {
        [Required(ErrorMessage = "Voer patiënt naam in.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Voer een gelding email in.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Voer een telefoonnummer in.")]
        [RegularExpression(@"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)", ErrorMessage = "Ongeldig telefoonnuummer")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Voer in geboortedatum van patiënt.")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Upload foto van patiënt.")]
        public IFormFile Photo { get; set; }
        [Required(ErrorMessage = "Selecteer geslacht van patiënt.")]
        public Gender? Gender { get; set; }
        [Required(ErrorMessage = "Selecteer rol van patiënt.")]
        public Role? Role { get; set; }
        [Required(ErrorMessage = "Voer in identificatienummer.")]
        public string IdentificationNumber { get; set; }
    }
}
