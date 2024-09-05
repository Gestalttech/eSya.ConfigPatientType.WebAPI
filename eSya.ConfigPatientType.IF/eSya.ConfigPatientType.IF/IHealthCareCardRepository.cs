using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface IHealthCareCardRepository
    {
        #region Care Card Details
        Task<List<DO_PatientTypeAttribute>> GetPatientTypesbyBusinessKey(int businesskey);
        Task<List<DO_PatientTypCategoryAttribute>> GetPatientCategoriesbyPatientType(int businesskey, int patienttypeID);
        Task<List<DO_HealthCareCard>> GetHealthCareCards(int businesskey, int patienttypeID, int patientcategoryID, int healthcardID);
        Task<List<DO_HealthCareCardSpecialtyLink>> GetSpecialtiesLinkedHealthCareCard(int businesskey, int healthcardID);
        Task<List<DO_HealthCareCardRates>> GetHealthCareCardRates(int businesskey, int healthcardID);
        Task<DO_ReturnParameter> InsertOrUpdateHealthCareCard(DO_HealthCareCard obj);
        Task<DO_ReturnParameter> InsertOrUpdateHealthCareCardRates(DO_HealthCareCardRates obj);
        #endregion
    }
}
