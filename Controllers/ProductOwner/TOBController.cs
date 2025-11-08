using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers.ProductOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class TOBController : ControllerBase
    {
        private readonly ITOBService _itobService;
        public TOBController(ITOBService tobService)
        {
            _itobService = tobService;
        }

        [HttpGet]
        [Route("GetTOBHistory/{HistoryId}")]
        public async Task<ActionResult> GetTOBHistory(long HistoryId)
        {
            try
            {
                var obj = await _itobService.GetTOBHistory(HistoryId);
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
        [Route("GetTOBList")]
        public async Task<ActionResult> GetTOBList()
        {
            try
            {
                var obj = await _itobService.GetTOBList();
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
        [Route("GetTOBMinorIndusryMapping")]
        public async Task<ActionResult> GetTOBMinorIndusryMapping(int tobId)
        {
            try
            {
                var obj = await _itobService.GetTOBMinorIndusryMapping(tobId);
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
        [Route("PostTOBMapping")]
        public async Task<ActionResult> PostTOBMapping([FromBody] TOBMappingList tobMapping)
        {
            try
            {
                var obj = await _itobService.PostTOBMapping(tobMapping);
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
        [Route("GetPendingTOBApproval/{UserUID}")]
        public async Task<ActionResult> GetPendingTOBApproval(Guid? UserUID)
        {
            try
            {
                var obj = await _itobService.GetPendingTOBApproval(UserUID);
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
        [Route("GetTOBMapping")]
        public async Task<ActionResult> GetTOBMapping()
        {
            try
            {
                var obj = await _itobService.GetTOBMappingList();
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
        [Route("GetTOBMappingByCountry")]
        public async Task<ActionResult> GetTOBMappingByCountry(long? countryId)
        {
            try
            {
                var obj = await _itobService.GetTOBMappingByCountry(countryId);
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
        [Route("GetTOBMappingByMajor")]
        public async Task<ActionResult> GetTOBMappingByMajor(long? majorInudustryId, long? countryId)
        {
            try
            {
                var obj = await _itobService.GetTOBMappingByMajor(majorInudustryId, countryId);
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
        [Route("GetTOBMappingByMinor")]
        public async Task<ActionResult> GetTOBMappingByMinor(long? minorInudustryId, long? majorInudustryId, int? countryId)
        {
            try
            {
                var obj = await _itobService.GetTOBMappingByMinor(minorInudustryId, majorInudustryId, countryId);
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
        [Route("GetHistoryTOB/{UID}")]
        public async Task<ActionResult> GetHistoryTOB(Guid? UID)
        {
            var obj = await _itobService.GetHistoryTOB(UID);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpGet]
        [Route("GetTOBMappingApproval/{UserUID}")]
        public async Task<ActionResult> GetTOBMappingApproval(Guid UserUID)
        {
            try
            {
                var obj = await _itobService.GetTOBMappingApprovaList(UserUID);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        //[Route("GetTOBApproval/{UserUID}")]
        //public async Task<ActionResult> GetTOBApproval(Guid UserUID)
        //{
        //    try
        //    {
        //        var obj = await _itobService.GetTOBApprovalList(UserUID);
        //        if (obj != null)
        //            return Ok(obj);
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("AddTOBDetails")]
        public async Task<ActionResult> AddTOBDetails([FromBody] TOB tobDetails)
        {
            try
            {
                var obj = await _itobService.AddTOBDetails(tobDetails);
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
        [Route("PostTOBMappingApprove")]
        public async Task<ActionResult> PostCountryMajorMappingApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _itobService.PostTOBMappingApprove(access);
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
        [Route("PostTOBMappingReject")]
        public async Task<ActionResult> PostCountryMajorMappingReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _itobService.PostTOBMappingReject(access);
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
        [Route("ApproveTOB")]
        public async Task<ActionResult> ApproveTOB([FromBody] AccessModel access)
        {
            var obj = await _itobService.ApproveTOB(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpPost]
        [Route("RejectTOB")]
        public async Task<ActionResult> RejectTOB([FromBody] AccessModel access)
        {
            var obj = await _itobService.RejectTOB(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpGet]
        [Route("GetNextTOBCode")]
        public async Task<ActionResult> GetNextTOBCode()
        {
            try
            {
                var code = await _itobService.GetNextTOBCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
