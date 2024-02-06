using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    #region Patient Type & Category Link with Param
    public interface IPatientTypesRepository
    {
        Task<DO_PatientAttributes> GetAllPatientTypesforTreeView(int CodeType);
        Task<DO_PatientTypCategoryAttribute> GetPatientCategoryInfo(int PatientTypeId, int PatientCategoryId);
        Task<DO_ReturnParameter> InsertPatientCategory(DO_PatientTypCategoryAttribute obj);
        Task<DO_ReturnParameter> UpdatePatientCategory(DO_PatientTypCategoryAttribute obj);
    }
    #endregion
}
