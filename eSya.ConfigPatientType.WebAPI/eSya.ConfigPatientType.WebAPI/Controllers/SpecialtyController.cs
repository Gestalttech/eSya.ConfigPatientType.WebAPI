using eSya.ConfigPatientType.DL.Repository;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtyController(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }
        #region Patient Type & Category Specialty Link
        [HttpGet]
        public async Task<IActionResult> GetPatientCategoriesforTreeViewbyPatientType(int PatientTypeId)
        {
            var rs = await _specialtyRepository.GetPatientCategoriesforTreeViewbyPatientType(PatientTypeId);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> GetPatientTypeCategorySpecialtyInfo(DO_PatientTypeCategorySpecialtyLink obj)
        {
            var rs = await _specialtyRepository.GetPatientTypeCategorySpecialtyInfo(obj.BusinessKey, obj.PatientTypeId, obj.PatientCategoryId);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePatientCategorySpecialtyLink(List<DO_PatientTypeCategorySpecialtyLink> obj)
        {
            var rs = await _specialtyRepository.InsertOrUpdatePatientCategorySpecialtyLink(obj);
            return Ok(rs);
        }
        #endregion
    }
}
