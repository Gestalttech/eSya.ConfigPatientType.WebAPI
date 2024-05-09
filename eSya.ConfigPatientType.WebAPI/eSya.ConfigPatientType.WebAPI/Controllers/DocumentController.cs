using eSya.ConfigPatientType.DO;
using eSya.ConfigPatientType.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigPatientType.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        #region Patient Type & Category Document Link
        //[HttpGet]
        //public async Task<IActionResult> GetAllPatientCategoryDocumentLink(int businesskey, int patienttypeId)
        //{
        //    var ds = await _documentRepository.GetAllPatientCategoryDocumentLink(businesskey, patienttypeId);
        //    return Ok(ds);
        //}
        [HttpPost]
        public async Task<IActionResult> GetPatientTypeCategoryDocumentInfo(DO_PatientTypeCategoryDocumentLink obj)
        {
            var ds = await _documentRepository.GetPatientTypeCategoryDocumentInfo(obj.BusinessKey, obj.PatientTypeId,obj.PatientCategoryId,obj.PatientCatgDocId);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePatientCategoryDocumentLink(List<DO_PatientTypeCategoryDocumentLink> obj)
        {
            var msg = await _documentRepository.InsertOrUpdatePatientCategoryDocumentLink(obj);
            return Ok(msg);
        }
        #endregion
    }
}
