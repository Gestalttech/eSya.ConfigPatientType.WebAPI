using eSya.ConfigPatientType.DL.Repository;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DependentController : ControllerBase
    {
        private readonly IDependentRepository _dependentRepository;

        public DependentController(IDependentRepository dependentRepository)
        {
            _dependentRepository = dependentRepository;
        }
        #region Patient Dependent

        [HttpGet]
        public async Task<IActionResult> GetAllDependents(int businesskey, int patienttypeID, int patientcategoryID)
        {
            var ds = await _dependentRepository.GetAllDependents(businesskey, patienttypeID, patientcategoryID);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePatientDependent(DO_Dependent obj)
        {
            var rs = await _dependentRepository.InsertOrUpdatePatientDependent(obj);
            return Ok(rs);
        }
        #endregion
    }
}
