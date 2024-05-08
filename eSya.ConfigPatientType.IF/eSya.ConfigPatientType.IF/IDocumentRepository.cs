using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface IDocumentRepository
    {
        #region Patient Type & Category Document Link
        Task<List<DO_PatientTypeCategoryDocumentLink>> GetAllPatientCategoryDocumentLink(int businesskey, int patienttypeId);
        Task<DO_ReturnParameter> InsertOrUpdatePatientCategoryDocumentLink(List<DO_PatientTypeCategoryDocumentLink> obj);
        #endregion
    }
}
