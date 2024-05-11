using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface IServiceTypeRepository
    {
        #region Patient Type & Category Service Type Link
        Task<DO_PatientAttributes> GetPatientCategoriesforTreeViewbyPatientType(int PatientTypeId);
        Task<List<DO_PatientTypeCategoryServiceTypeLink>> GetPatientTypeCategoryServiceTypeInfo(int businesskey, int PatientTypeId, int PatientCategoryId);
        Task<DO_ReturnParameter> InsertPatientTypeCategoryServiceTypeLink(DO_PatientTypeCategoryServiceTypeLink obj);
        Task<DO_ReturnParameter> UpdatePatientTypeCategoryServiceTypeLink(DO_PatientTypeCategoryServiceTypeLink obj);
        Task<List<DO_PatientTypeCategoryServiceTypeLink>> GetActiveServiceTypes();
        #endregion
    }
}
