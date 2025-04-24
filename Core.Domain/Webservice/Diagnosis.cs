using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Webservice
{
    public class Diagnosis
    {
        public int Id { get; set; }
        [Required]
        public string DCSPH { get; set; }
        [Required]
        public string BodyLocalization { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
