using eSya.ConfigPatientType.DL.Repository;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        
        private readonly IServiceTypeRepository _serviceTypeRepository;

        public ServiceTypeController(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }
        #region Patient Type & Category Service Type Link
        [HttpGet]
        public async Task<IActionResult> GetPatientCategoriesforTreeViewbyPatientType(int PatientTypeId)
        {
            var rs = await _serviceTypeRepository.GetPatientCategoriesforTreeViewbyPatientType(PatientTypeId);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveServiceTypes()
        {
            var stype = await _serviceTypeRepository.GetActiveServiceTypes();
            return Ok(stype);
        }
        [HttpPost]
        public async Task<IActionResult> GetPatientTypeCategoryServiceTypeInfo(DO_PatientTypeCategoryServiceTypeLink obj)
        {
            var rs = await _serviceTypeRepository.GetPatientTypeCategoryServiceTypeInfo(obj.BusinessKey,obj.PatientTypeId,obj.PatientCategoryId);
            return Ok(rs);
        }
        [HttpPost]
        public async Task<IActionResult> InsertPatientTypeCategoryServiceTypeLink(DO_PatientTypeCategoryServiceTypeLink obj)
        {
            var rs = await _serviceTypeRepository.InsertPatientTypeCategoryServiceTypeLink(obj);
            return Ok(rs);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePatientTypeCategoryServiceTypeLink(DO_PatientTypeCategoryServiceTypeLink obj)
        {
            var rs = await _serviceTypeRepository.UpdatePatientTypeCategoryServiceTypeLink(obj);
            return Ok(rs);
        }
        #endregion
    }
}
