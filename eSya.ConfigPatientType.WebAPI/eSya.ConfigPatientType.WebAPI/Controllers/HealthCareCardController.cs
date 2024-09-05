using eSya.ConfigPatientType.DL.Repository;
using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HealthCareCardController : ControllerBase
    {
        private readonly IHealthCareCardRepository _healthCareCardRepository;

        public HealthCareCardController(IHealthCareCardRepository healthCareCardRepository)
        {
            _healthCareCardRepository = healthCareCardRepository;
        }
        #region Care Card Details
        [HttpGet]
        public async Task<IActionResult> GetPatientTypesbyBusinessKey(int businesskey)
        {
            var ds = await _healthCareCardRepository.GetPatientTypesbyBusinessKey(businesskey);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetPatientCategoriesbyPatientType(int businesskey, int patienttypeID)
        {
            var ds = await _healthCareCardRepository.GetPatientCategoriesbyPatientType(businesskey, patienttypeID);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetHealthCareCards(int businesskey, int patienttypeID, int patientcategoryID, int healthcardID)
        {
            var ds = await _healthCareCardRepository.GetHealthCareCards(businesskey, patienttypeID, patientcategoryID, healthcardID);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetSpecialtiesLinkedHealthCareCard(int businesskey, int healthcardID)
        {
            var ds = await _healthCareCardRepository.GetSpecialtiesLinkedHealthCareCard(businesskey, healthcardID);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetHealthCareCardRates(int businesskey, int healthcardID)
        {
            var ds = await _healthCareCardRepository.GetHealthCareCardRates(businesskey, healthcardID);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateHealthCareCard(DO_HealthCareCard obj)
        {
            var msg = await _healthCareCardRepository.InsertOrUpdateHealthCareCard(obj);
            return Ok(msg);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateHealthCareCardRates(DO_HealthCareCardRates obj)
        {
            var msg = await _healthCareCardRepository.InsertOrUpdateHealthCareCardRates(obj);
            return Ok(msg);
        }
        #endregion
    }
}
