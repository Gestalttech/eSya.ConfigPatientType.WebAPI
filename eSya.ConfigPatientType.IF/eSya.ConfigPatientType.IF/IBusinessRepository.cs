using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface IBusinessRepository
    {
        #region Patient Type & Category Business Link
        //Task<List<DO_PatientTypeCategoryBusinessLink>> GetAllPatientCategoryBusinessLink(int businesskey);
        Task<List<DO_PatientTypeCategoryBusinessLink>> GetAllPatientCategoryBusinessLink(int businesskey, int patienttypeId);
        Task<DO_ReturnParameter> InsertOrUpdatePatientCategoryBusinessLink(List<DO_PatientTypeCategoryBusinessLink> obj);
        #endregion
    }
}
