using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Treatment
{
    public class TreatmentPlan
    {
        public int Id { get; set; }
        [Required]
        public int SessionsPerWeek { get; set; }
        [Required]
        public int SessionDuration { get; set; }
    }
}
