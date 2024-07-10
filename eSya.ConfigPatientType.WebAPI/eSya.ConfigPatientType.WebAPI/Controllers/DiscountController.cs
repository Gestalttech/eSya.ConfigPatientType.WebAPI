using eSya.ConfigPatientType.DL.Repository;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IPatientCategoryDiscountRepository _patientCategoryDiscountRepository;

        public DiscountController(IPatientCategoryDiscountRepository patientCategoryDiscountRepository)
        {
            _patientCategoryDiscountRepository = patientCategoryDiscountRepository;
        }

        #region Patient Category Discount
        [HttpGet]
        public async Task<IActionResult> GetActivePatientCategoriesbyBusinessKey(int businesskey)
        {
            var ds = await _patientCategoryDiscountRepository.GetActivePatientCategoriesbyBusinessKey(businesskey);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetPatientCategoryDiscountbyDiscountAt(int businesskey, int patientcategoryId, int discountfor, bool serviceclass)
        {
            var ds = await _patientCategoryDiscountRepository.GetPatientCategoryDiscountbyDiscountAt(businesskey, patientcategoryId, discountfor, serviceclass);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertPatientCategoryDiscount(DO_PatientCategoryDiscount obj)
        {
            var ds = await _patientCategoryDiscountRepository.InsertPatientCategoryDiscount(obj);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePatientCategoryDiscount(DO_PatientCategoryDiscount obj)
        {
            var ds = await _patientCategoryDiscountRepository.UpdatePatientCategoryDiscount(obj);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> GetPatientPatientCategoryDiscountInfo(DO_PatientCategoryDiscount obj)
        {
            var ds = await _patientCategoryDiscountRepository.GetPatientPatientCategoryDiscountInfo(obj);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActivePatientCategoryDiscount(bool status, DO_PatientCategoryDiscount obj)
        {
            var ds = await _patientCategoryDiscountRepository.ActiveOrDeActivePatientCategoryDiscount(status,obj);
            return Ok(ds);
        }
        #endregion
    }
}
