﻿using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonMethodController : ControllerBase
    {
        private readonly ICommonDataRepository _commonDataRepository;
        public CommonMethodController(ICommonDataRepository commonDataRepository)
        {
            _commonDataRepository = commonDataRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesbyCodeType(int codetype)
        {
            var appcodes = await _commonDataRepository.GetApplicationCodesbyCodeType(codetype);
            return Ok(appcodes);
        }
        [HttpPost]
        public async Task<IActionResult> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            var ac = await _commonDataRepository.GetApplicationCodesByCodeTypeList(l_codeType);
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var appcodes = await _commonDataRepository.GetBusinessKey();
            return Ok(appcodes);
        }
        [HttpGet]
        public async Task<IActionResult> GetActivePatientTypes()
        {
            var pts = await _commonDataRepository.GetActivePatientTypes();
            return Ok(pts);
        }
        [HttpGet]
        public async Task<IActionResult> GetPatientCategory()
        {
            var pts = await _commonDataRepository.GetPatientCategory();
            return Ok(pts);
        }
        [HttpGet]
        public async Task<IActionResult> GetActiveCurrencies()
        {
            var pts = await _commonDataRepository.GetActiveCurrencies();
            return Ok(pts);
        }
    }
}
