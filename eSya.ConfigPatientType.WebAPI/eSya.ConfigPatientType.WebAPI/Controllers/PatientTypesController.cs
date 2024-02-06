using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientTypesController : ControllerBase
    {
        private readonly IPatientTypesRepository _PatientTypesRepository;

        public PatientTypesController(IPatientTypesRepository PatientTypesRepository)
        {
            _PatientTypesRepository = PatientTypesRepository;
        }
        #region Patient Type & Category Link with Param

        [HttpGet]
        public async Task<IActionResult> GetAllPatientTypesforTreeView(int CodeType)
        {
            var rs = await _PatientTypesRepository.GetAllPatientTypesforTreeView(CodeType);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientCategoryInfo(int PatientTypeId, int PatientCategoryId)
        {
            var rs = await _PatientTypesRepository.GetPatientCategoryInfo(PatientTypeId, PatientCategoryId);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPatientCategory(DO_PatientTypCategoryAttribute obj)
        {
            var rs = await _PatientTypesRepository.InsertPatientCategory(obj);
            return Ok(rs);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePatientCategory(DO_PatientTypCategoryAttribute obj)
        {
            var rs = await _PatientTypesRepository.UpdatePatientCategory(obj);
            return Ok(rs);
        }
        #endregion
    }
}
