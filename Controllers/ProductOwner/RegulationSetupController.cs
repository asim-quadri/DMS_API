using System.Linq;
using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers.ProductOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegulationSetupController : ControllerBase
    {
        private readonly IRegulationSetupService _regulationsetupService;
        private readonly IAccessServices _accessServices;

        public RegulationSetupController(IRegulationSetupService regulationsetupService, IAccessServices accessServices)
        {
            _regulationsetupService = regulationsetupService;
            _accessServices = accessServices;
        }

        [HttpGet]
        [Route("GetRegulationSetupDetails/{regSetupuid}")]
        public async Task<ActionResult> GetRegulationSetupDetails(Guid? regSetupuid)
        {
            try
            {
                var obj = await _regulationsetupService.GetRegulationSetupDetails(regSetupuid);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllRegulationSetupDetails")]
        public async Task<ActionResult> GetAllRegulationSetupDetails()
        {
            try
            {
                var obj = await _regulationsetupService.GetAllRegulationSetupDetails();
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetRegSetupHistory")]
        public async Task<ActionResult> GetRegSetupHistory(int? userId = null)
        {
            try
            {
                var obj = await _regulationsetupService.GetRegSetupHistory();

                if (obj == null)
                {
                    return NotFound();
                }
                
                if (!userId.HasValue && obj != null)
                    return Ok(obj);
                var userRgulations = await _accessServices.GetUserRegulationMappingDetailsByUserIdAsync(userId.Value);
                if (userRgulations == null)
                {
                    return Ok(obj);
                }
                if (!userRgulations.Regulations.Any())
                {
                    return Ok(obj);
                }
                var regulationIds = userRgulations.Regulations
                                        .Where(r => r.Id != null)
                                        .Select(r => r.Id)
                                        .ToHashSet();

                var res = obj.Where(o => o.Id != null && regulationIds.Contains(o.Id.ToString())).ToList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetHistoryRegSetup/{UID}")]
        public async Task<ActionResult> GetHistoryRegSetup(Guid? UID)
        {
            var obj = await _regulationsetupService.GetHistoryRegulationSetup(UID);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpPost]
        [Route("PostApproveAccess")]
        public async Task<ActionResult> PostApproveAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationsetupService.PostParameterApproveAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostRejectAccess")]
        public async Task<ActionResult> PostRejectAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationsetupService.PostParameterRejectAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostReviewedAccess")]
        public async Task<ActionResult> PostReviewedAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationsetupService.PostParameterReviewAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRegulationSetupParameters")]
        public async Task<ActionResult> GetRegulationSetupParameters()
        {
            try
            {
                var obj = await _regulationsetupService.GetAllRegulationSetupParameter();
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddRegulationSetupDetails")]
        public async Task<ActionResult> AddRegulationSetupDetails([FromBody] RegulationStupDetails regulationStupDetails)
        {
            try
            {
                var obj = await _regulationsetupService.AddRegulationSetupDetails(regulationStupDetails);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ApproveRegulationSetup")]
        public async Task<ActionResult> ApproveRegulationSetup([FromBody] AccessModel access)
        {
            var obj = await _regulationsetupService.ApproveRegulationSetup(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpPost]
        [Route("RejectRegulationSetup")]
        public async Task<ActionResult> RejectRegulationSetup([FromBody] AccessModel access)
        {
            var obj = await _regulationsetupService.RejectRegulationSetup(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetMinorIndustrybyMajorID")]
        public async Task<ActionResult> GetMinorIndustrybyMajorID(long majorIndustoryId)
        {
            try
            {
                var obj = await _regulationsetupService.GetMinorIndustrybyMajorID(majorIndustoryId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetMinorIndustrybyMajorIDMap")]
        public async Task<ActionResult> GetMinorIndustrybyMajorIDMap([FromBody] List<long> majorIndustoryId, long countryId)
        {
            try
            {
                var obj = await _regulationsetupService.GetMinorIndustrybyMajorIDMap(majorIndustoryId, countryId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetTOBMinorIndustrybyMajorIDMap")]
        public async Task<ActionResult> GetTOBMinorIndustrybyMajorIDMap([FromBody] TOBMinorIndustryRequest request)
        {
            try
            {
                var obj = await _regulationsetupService.GetTOBMinorIndustrybyMajorIDMap(request.MajorIndustryIds!, request.MinorIndustryIds!, request.CountryId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetIndustryMapping")]
        public async Task<ActionResult> GetIndustryMapping(long countryId)
        {
            try
            {
                var obj = await _regulationsetupService.GetIndustryMapping(countryId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("AddRegulationSetupParameters")]
        //public async Task<ActionResult> AddRegulationSetupParameters([FromBody] AddRegulationStupParameters regulationStupParameters, Guid? accessUID)
        //{
        //    try
        //    {
        //        var obj = await _regulationsetupService.AddRegulationSetupParameters(regulationStupParameters, accessUID);
        //        if (obj != null)
        //            return Ok(obj);
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        [Route("GetRegulationSetupCompliance/{complianceuid}")]
        public async Task<ActionResult> GetRegulationSetupCompliance(Guid? complianceuid)
        {
            var obj = await _regulationsetupService.GetRegulationSetupCompliance(complianceuid);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpPost]
        [Route("AddRegSetupComplianceAsync")]
        public async Task<ActionResult> AddRegSetupComplianceAsync([FromBody] RegSetupComplianceModel regulationStupComplianceParameters)
        {
            try
            {
                var obj = await _regulationsetupService.AddRegSetupComplianceAsync(regulationStupComplianceParameters);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllRegulationBasicAndParameterByCountryID/{countryId}")]
        public async Task<ActionResult> GetAllRegulationBasicAndParameterByCountryID(int countryId)
        {
            var obj = await _regulationsetupService.GetAllRegulationBasicAndParameterByCountryID(countryId);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetPendingRegulationSetupApproval/{UserUID}")]
        public async Task<ActionResult> GetPendingRegulationSetupApproval(Guid? UserUID)
        {
            var obj = await _regulationsetupService.GetPendingRegulationSetupApproval(UserUID);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetRegSetupComplianceHistory/{UID}")]
        public async Task<ActionResult> GetRegSetupComplianceHistory(Guid? UID)
        {
            var obj = await _regulationsetupService.GetRegSetupComplianceHistory(UID);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpPost]
        [Route("ApproveRegSetupCompliance")]
        public async Task<ActionResult> ApproveRegSetupCompliance([FromBody] AccessModel access)
        {
            var obj = await _regulationsetupService.ApproveRegSetupCompliance(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpPost]
        [Route("RejectRegSetupCompliance")]
        public async Task<ActionResult> RejectRegSetupCompliance([FromBody] AccessModel access)
        {
            var obj = await _regulationsetupService.RejectRegSetupCompliance(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetTOCRegistration/{tocRegId}/{complianceId}/{regulationid}/{ruleType}")]
        public async Task<ActionResult> GetTOCRegistration(long tocRegId, long? complianceId, long? regulationid, string ruleType)
        {
            try
            {
                var obj = await _regulationsetupService.GetTOCRegistration(tocRegId, complianceId, regulationid, ruleType);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostTOCRegistration")]
        public async Task<ActionResult> PostTOCRegistration([FromBody] TOCRegistrationModel model)
        {
            try
            {
                var obj = await _regulationsetupService.SaveTOCRegistrationAsync(model);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTOCRulesAsync/{tocId}/{regulationid}/{ruleType}")]
        public async Task<ActionResult> GetTOCRulesAsync(long tocId, long regulationid, string ruleType)
        {
            try
            {
                var obj = await _regulationsetupService.GetTOCRulesAsync(tocId, regulationid, ruleType);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostTOCRules")]
        public async Task<ActionResult> PostTOCRules([FromBody] TOCRules model)
        {
            try
            {
                var obj = await _regulationsetupService.PostTOCRules(model);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTOCHistory/{historyId}")]
        public async Task<ActionResult> GetTOCHistory(long historyId)
        {
            var obj = await _regulationsetupService.GetTOCHistory(historyId);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpPost]
        [Route("ApproveTOCRegistration")]
        public async Task<ActionResult> ApproveTOCRegistration([FromBody] AccessModel access)
        {
            var obj = await _regulationsetupService.ApproveTOCRegistration(access);
            return Ok(obj);
        }

        [HttpPost]
        [Route("ApproveTOCRule")]
        public async Task<ActionResult> ApproveTOCRule([FromBody] AccessModel access)
        {
            var obj = await _regulationsetupService.ApproveTOCRule(access);
            return Ok(obj);
        }

        [HttpPost]
        [Route("RejectTOCRegistration")]
        public async Task<ActionResult> RejectTOCRegistration([FromBody] AccessModel access)
        {
            var obj = await _regulationsetupService.RejectTOC(access);
            return Ok(obj);
        }

        [HttpGet]
        [Route("GetAllRegisteration")]
        public async Task<ActionResult> GetAllRegisterations()
        {
            var obj = await _regulationsetupService.GetTOCRegisteration();
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }
        [HttpGet]
        [Route("GetRegulationsByCountryIds")]
        public async Task<ActionResult> GetRegulationsByCountryIds(string countryIds)
        {
            try
            {
                var regulations = await _regulationsetupService.GetRegulationsByCountryIds(countryIds);
                if (regulations != null && regulations.Any())
                    return Ok(regulations);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetNextRegulationSetupCode")]
        public async Task<ActionResult> GetNextRegulationSetupCode()
        {
            try
            {
                var code = await _regulationsetupService.GetNextRegulationSetupCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetNextRegulationSetupComplianceCode")]
        public async Task<IActionResult> GetNextRegulationSetupComplianceCode()
        {
            var code = await _regulationsetupService.GetNextRegulationSetupComplianceCode();
            return Ok(code);
        }

        [HttpGet]
        [Route("GetNextRegulationSetupTypeOfComplianceRegisterCode")]
        public async Task<IActionResult> GetNextRegulationSetupTypeOfComplianceRegisterCode()
        {
            var code = await _regulationsetupService.GetNextRegulationSetupTypeOfComplianceRegisterCode();
            return Ok(code);
        }

        [HttpGet]
        [Route("GetNextRegulationSetupTypeOfComplianceDuesCode")]
        public async Task<IActionResult> GetNextRegulationSetupTypeOfComplianceDuesCode()
        {
            var code = await _regulationsetupService.GetNextRegulationSetupTypeOfComplianceDuesCode();
            return Ok(code);
        }

        [HttpGet("TypeOfComplianceByRegulationSetupIdComplianceId")]
        public async Task<IActionResult> TypeOfComplianceByRegulationSetupIdComplianceId(int? regulationSetupId, int? complianceId)
        {
            try
            {
                var obj = await _regulationsetupService.GetTypeOfCompliances();
                if (obj == null)
                {
                    return NotFound();
                }
                obj = obj.Where(o => o.RegulationSetupId == regulationSetupId || o.ComplianceId == complianceId).ToList();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllTOCDues")]
        public async Task<ActionResult> GetAllTOC()
        {
            try
            {
                var obj = await _regulationsetupService.GetAllTOCDuesAsync();
                if (obj != null && obj.Any())
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}