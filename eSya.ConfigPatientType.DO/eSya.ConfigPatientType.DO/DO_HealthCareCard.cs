using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.DO
{
    public class DO_HealthCareCard
    {
        public int BusinessKey { get; set; }
        public int PatientTypeId { get; set; }
        public int PatientCategoryId { get; set; }
        public int HealthCardId { get; set; }
        public DateTime OfferStartDate { get; set; }
        public DateTime? OfferEndDate { get; set; }
        public int CardValidityInMonths { get; set; }
        public string CareCardNoPattern { get; set; } = null!;
        public bool IsSpecialtySpecific { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public List<DO_HealthCareCardSpecialtyLink>? lstspecialty { get;set; }
    }
    public class DO_HealthCareCardSpecialtyLink
    {
        public int BusinessKey { get; set; }
        public int HealthCardId { get; set; }
        public int SpecialtyId { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? SpecialtyDesc { get;set; }
    }
    public class DO_HealthCareCardRates
    {
        public int BusinessKey { get; set; }
        public int HealthCardId { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public decimal CardCharges { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? CurrencyName { get; set; }
    }
    public class DO_CurrencyCodes
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
    }
}
