using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.DO
{
    public class DO_PatientTypCategoryAttribute
    {
        public int PatientTypeId { get; set; }
        public int PatientCategoryId { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? Description { get; set; }
        public List<DO_eSyaParameter> l_ptypeparams { get; set; }
    }
    public class DO_PatientAttributes
    {
        public List<DO_PatientTypeAttribute> l_PatientType { get; set; }
        public List<DO_PatientTypCategoryAttribute> l_PatienTypeCategory { get; set; }
    }
    public class DO_PatientTypeAttribute
    {
        public int PatientTypeId { get; set; }
        public string Description { get; set; }
        public bool ActiveStatus { get; set; }
    }
    public class DO_eSyaParameter
    {
        public int ParameterID { get; set; }
        public string? ParameterValue { get; set; }
        public bool ParmAction { get; set; }
        public decimal ParmValue { get; set; }
        public decimal ParmPerc { get; set; }
        public string? ParmDesc { get; set; }
        public bool ActiveStatus { get; set; }
    }
}
