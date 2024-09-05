using eSya.ConfigPatientType.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigPatientType.IF
{
    public interface IDependentRepository
    {
        #region Patient Dependent
        Task<List<DO_Dependent>> GetAllDependents(int businesskey, int patienttypeID, int patientcategoryID);
        Task<DO_ReturnParameter> InsertOrUpdatePatientDependent(DO_Dependent obj);
        #endregion
    }
}
