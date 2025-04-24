using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain.People;

namespace Core.Domain.PatientRecord
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string CommentText { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public bool Visible { get; set; }
        public int PlacedById { get; set; }
        public Worker PlacedBy { get; set; }
        public int PatientRecordId { get; set; }
        public Domain.PatientRecord.PatientRecord PatientRecord { get; set; }
    }
}
