using dto = ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustryController : ControllerBase
    {
        private readonly IIndustryService _industryServices;

        public IndustryController(IIndustryService industryServices)
        {
            _industryServices = industryServices;
        }

        [HttpGet]
        [Route("GetAllMajorIndustries")]
        public async Task<ActionResult> GetAllMajorIndustries()
        {
            try
            {
                var obj = await _industryServices.GetAllMajorIndustry();
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
        [Route("GetMajorIndustryById/{CountryId}")]
        public async Task<ActionResult> GetMajorIndustryById(int CountryId)
        {
            try
            {
                var obj = await _industryServices.GetMajorIndustryById(CountryId);
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
        [Route("GetMinorIndustryById/{majorIndustryId}")]
        public async Task<ActionResult> GetMinorIndustryById(int majorIndustryId)
        {
            try
            {
                var obj = await _industryServices.GetMinorIndustryById(majorIndustryId);
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
        [Route("GetMajorIndustryByUID/{uid}")]
        public async Task<ActionResult> GetMajorIndustryByUID(Guid uid)
        {
            try
            {
                var obj = await _industryServices.GetMajorIndustryByUID(uid);
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
        [Route("PostMajorIndustry")]
        public async Task<ActionResult> PostMajorIndustry([FromBody] dto.MajorIndustry majorIndustry)
        {
            try
            {
                var obj = await _industryServices.PostMajorIndustry(majorIndustry);
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
        [Route("PostMinorIndustry")]
        public async Task<ActionResult> PostMinorIndustry([FromBody] dto.MinorIndustry minorIndustry)
        {
            try
            {
                var obj = await _industryServices.PostMinorIndustry(minorIndustry);
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
        [Route("GetCountryMajorMapping")]
        public async Task<ActionResult> GetCountryMajorMapping()
        {
            try
            {
                var obj = await _industryServices.GetCountryMajorMapping();
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
        [Route("PostCountryMajorMapping")]
        public async Task<ActionResult> PostCountryMajorMapping([FromBody] dto.IndustryrMapping industryMapping)
        {
            try
            {
                var obj = await _industryServices.PostIndustryMapping(industryMapping);
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
        [Route("GetMajorMinorMapping")]
        public async Task<ActionResult> GetMajorMinorMapping()
        {
            try
            {
                var obj = await _industryServices.GetMajorMinorMapping();
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
        [Route("PostMajorMinorMapping")]
        public async Task<ActionResult> PostMajorMinorMapping([FromBody] dto.MajorMinorMapping majorMinorMapping)
        {
            try
            {
                var obj = await _industryServices.PostMajorMinorMapping(majorMinorMapping);
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
        [Route("GetIndustryMappingApproval/{UserUID}")]
        public async Task<ActionResult> GetIndustryMappingApproval(Guid UserUID)
        {
            try
            {
                var obj = await _industryServices.GetIndustryMappingApprovaList(UserUID);
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
        public async Task<ActionResult> GetIndustryMapping()
        {
            try
            {
                var obj = await _industryServices.GetIndustryMapping();
                if (obj == null)
                {
                    return NotFound();
                }
                var res = obj.GroupBy(x => x.MajorIndustryId)
                      .SelectMany(g => g)
                      .ToList();

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetIndustryMappingByMajor")]
        public async Task<ActionResult> GetIndustryMappingByMajor(long? majorInudustryId, long? countryId)
        {
            try
            {
                var obj = await _industryServices.GetIndustryMappingByMajor(majorInudustryId, countryId);
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
        [Route("GetIndustryMappingByCountry")]
        public async Task<ActionResult> GetIndustryMappingByCountry(long? countryId)
        {
            try
            {
                var obj = await _industryServices.GetIndustryMappingByCountry(countryId);
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
        [Route("GetIndustryApproval/{UserUID}")]
        public async Task<ActionResult> GetIndustryApproval(Guid UserUID)
        {
            try
            {
                var obj = await _industryServices.GetIndustryApprovaList(UserUID);
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
        [Route("GetMajorIndustryApproval/{UserUID}")]
        public async Task<ActionResult> GetMajorIndustryApproval(Guid UserUID)
        {
            try
            {
                var obj = await _industryServices.GetMajorIndustryApprovaList(UserUID);
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
        [Route("GetMinorIndustryApproval/{UserUID}")]
        public async Task<ActionResult> GetMinorIndustryApproval(Guid UserUID)
        {
            try
            {
                var obj = await _industryServices.GetMinorIndustryApprovaList(UserUID);
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
        [Route("GetCountryMajorMappingApproval/{UserUID}")]
        public async Task<ActionResult> GetCountryMajorMappingApproval(Guid UserUID)
        {
            try
            {
                var obj = await _industryServices.GetCountryMajorMappingApprovaList(UserUID);
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
        [Route("GetMajorMinorMappingApproval/{UserUID}")]
        public async Task<ActionResult> GetMajorMinorMappingApproval(Guid UserUID)
        {
            try
            {
                var obj = await _industryServices.GetMajorMinorMappingApprovaList(UserUID);
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
        [Route("PostMajorIndustryApprove")]
        public async Task<ActionResult> PostMajorIndustryApprove([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMajorIndustryApprove(access);
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
        [Route("PostMajorIndustryReject")]
        public async Task<ActionResult> PostMajorIndustryReject([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMajorIndustryReject(access);
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
        [Route("PostMajorIndustryForward")]
        public async Task<ActionResult> PostMajorIndustryForward([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMajorIndustryForward(access);
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
        [Route("PostMinorIndustryApprove")]
        public async Task<ActionResult> PostMinorIndustryApprove([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMinorIndustryApprove(access);
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
        [Route("PostMinorIndustryReject")]
        public async Task<ActionResult> PostMinorIndustryReject([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMinorIndustryReject(access);
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
        [Route("PostMinorIndustryForward")]
        public async Task<ActionResult> PostMinorIndustryForward([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMinorIndustryForward(access);
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
        [Route("PostCountryMajorApprove")]
        public async Task<ActionResult> PostCountryMajorMappingApprove([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostIndustryMappingApprove(access);
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
        [Route("PostCountryMajorReject")]
        public async Task<ActionResult> PostCountryMajorMappingReject([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostIndustryMappingReject(access);
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
        [Route("PostMajorMinorApprove")]
        public async Task<ActionResult> PostMajorMinorMappingApprove([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMajorMinorMappingApprove(access);
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
        [Route("PostMajorMinorReject")]
        public async Task<ActionResult> PostMajorMinorMappingReject([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMajorMinorMappingReject(access);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("PostCountryMajorIndustryForward")]
        //public async Task<ActionResult> PostCountryMajorIndustryForward([FromBody] AccessModel access)
        //{
        //    try
        //    {
        //        var obj = await _industryServices.PostCountryMajorIndustryForward(access);
        //        if (obj != null)
        //            return Ok(obj);
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("PostMajorMinorIndustryForward")]
        public async Task<ActionResult> PostMajorMinorIndustryForward([FromBody] dto.AccessModel access)
        {
            try
            {
                var obj = await _industryServices.PostMajorMinorIndustryForward(access);
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
        [Route("GetNextMajorIndustryCode")]
        public async Task<ActionResult> GetNextMajorIndustryCode()
        {
            try
            {
                var code = await _industryServices.GetNextMajorIndustryCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetNextMinorIndustryCode")]
        public async Task<ActionResult> GetNextMinorIndustryCode()
        {
            try
            {
                var code = await _industryServices.GetNextMinorIndustryCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}