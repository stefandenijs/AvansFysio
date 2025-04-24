using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Models.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Voer in een geldig email.")]
        [EmailAddress(ErrorMessage = "Voer in een geldig email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Voer in een wachtwoord.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Voer wachtwoord nogmaals in.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overheen.")]
        public string ConfirmedPassword { get; set; }
    }
}
