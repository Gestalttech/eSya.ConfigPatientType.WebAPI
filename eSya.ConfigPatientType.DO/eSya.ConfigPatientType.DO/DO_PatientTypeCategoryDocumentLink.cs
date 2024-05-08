using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.DO
{
    public class DO_PatientTypeCategoryDocumentLink
    {
        public int BusinessKey { get; set; }
        public int PatientTypeId { get; set; }
        public int PatientCategoryId { get; set; }
        public int PatientCatgDocId { get; set; } 
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? PatientTypeDesc { get; set; }
        public string? PatientCategoryDesc { get; set; }
    }
}
