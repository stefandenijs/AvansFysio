using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Webservice
{
    public class TreatmentInfo
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ExplanationRequired { get; set; }
    }
}
