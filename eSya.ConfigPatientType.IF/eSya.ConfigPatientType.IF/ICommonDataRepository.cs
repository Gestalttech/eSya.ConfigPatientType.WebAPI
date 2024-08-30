using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface ICommonDataRepository
    {
        Task<List<DO_ApplicationCodes>> GetApplicationCodesbyCodeType(int codetype);
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeTypeList(List<int> l_codeType);
        Task<List<DO_BusinessLocation>> GetBusinessKey();
        Task<List<DO_PatientTypCategoryAttribute>> GetActivePatientTypes();
        Task<List<DO_ApplicationCodes>> GetPatientCategory();
    }
}
