using System;
using System.Collections.Generic;

namespace eSya.ConfigPatientType.DL.Entities
{
    public partial class GtEcptcd
    {
        public int BusinessKey { get; set; }
        public int PatientTypeId { get; set; }
        public int PatientCategoryId { get; set; }
        public int PatientCatgDocId { get; set; }
        public string PatientCatgDocDesc { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
