﻿using System;
using System.Collections.Generic;

namespace eSya.ConfigPatientType.DL.Entities
{
    public partial class GtEcptc
    {
        public int BusinessKey { get; set; }
        public int HealthCardId { get; set; }
        public int SpecialtyId { get; set; }
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
