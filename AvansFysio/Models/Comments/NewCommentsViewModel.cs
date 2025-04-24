using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysio.Models.Comments
{
    public class NewCommentsViewModel
    {
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Voer in een tekst.")]
        public string Comment { get; set; }
        [Required]
        public bool Visible { get; set; }
    }
}
