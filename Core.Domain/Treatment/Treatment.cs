using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain.PatientRecord;
using Core.Domain.People;
using Core.Domain.Space;

namespace Core.Domain.Treatment
{
    public class Treatment
    {
        public int Id { get; set; }
        [Required]
        public int TreatmentInfoId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string Particularities { get; set; }
        public int TreatmentPerformedById { get; set; }
        public Worker TreatmentPerformedBy { get; set; }
        public int PatientRecordId { get; set; }
        public PatientRecord.PatientRecord PatientRecord { get; set; }
        [Required]
        public DateTime TreatmentPerformedDate { get; set; }
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
