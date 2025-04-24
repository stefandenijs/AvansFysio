using System.ComponentModel.DataAnnotations;

namespace Core.Domain.People
{
    public class Intern : Worker
    {
        [Required]
        public string StudentNumber { get; set; }
    }
}
