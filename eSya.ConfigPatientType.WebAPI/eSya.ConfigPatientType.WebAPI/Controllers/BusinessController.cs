using eSya.ConfigPatientType.DL.Repository;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessRepository _businessRepository;

        public BusinessController(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
        }
        #region Patient Type & Category Business Link
        //[HttpGet]
        //public async Task<IActionResult> GetAllPatientCategoryBusinessLink(int businesskey)
        //{
        //    var ds = await _businessRepository.GetAllPatientCategoryBusinessLink(businesskey);
        //    return Ok(ds);
        //}
        [HttpGet]
        public async Task<IActionResult> GetAllPatientCategoryBusinessLink(int businesskey, int patienttypeId)
        {
            var ds = await _businessRepository.GetAllPatientCategoryBusinessLink(businesskey, patienttypeId);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePatientCategoryBusinessLink(List<DO_PatientTypeCategoryBusinessLink> obj)
        {
            var msg = await _businessRepository.InsertOrUpdatePatientCategoryBusinessLink(obj);
            return Ok(msg);
        }
        #endregion
    }
}
