using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface ISpecialtyRepository
    {
        #region Patient Type & Category Specialty Link
        Task<DO_PatientAttributes> GetPatientCategoriesforTreeViewbyPatientType(int patienttype);
        Task<List<DO_PatientTypeCategorySpecialtyLink>> GetPatientTypeCategorySpecialtyInfo(int businesskey, int PatientTypeId, int PatientCategoryId);
        Task<DO_ReturnParameter> InsertOrUpdatePatientCategorySpecialtyLink(List<DO_PatientTypeCategorySpecialtyLink> obj);
        #endregion
    }
}
