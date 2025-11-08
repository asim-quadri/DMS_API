using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegulationGroupController : ControllerBase
    {
        private readonly IRegulationGroupService _regulationGroup;

        public RegulationGroupController(IRegulationGroupService regulationGroup)
        {
            _regulationGroup = regulationGroup;
        }

        [HttpGet]
        [Route("GetAllRegulationGroups")]
        public async Task<ActionResult> GetAllRegulationGroups()
        {
            try
            {
                var obj = await _regulationGroup.GetAllRegulationGroups();
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
        [Route("GetRegulationGroupByUID/{uid}")]
        public async Task<ActionResult> GetRegulationGroupByUID(Guid uid)
        {
            try
            {
                var obj = await _regulationGroup.GetRegulationGroupByUID(uid);
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
        [Route("PostRegulationGroup")]
        public async Task<ActionResult> PostRegulationGroup([FromBody] RegulationGroupModel regulation)
        {
            try
            {
                var obj = await _regulationGroup.PostRegulationGroup(regulation);
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
        [Route("GetCountryRegulationGroupMapping")]
        public async Task<ActionResult> GetCountryRegulationGroupMapping(int? countryId = null)
        {
            try
            {
                var obj = await _regulationGroup.GetCountryRegulationGroupMapping();
                if (obj == null)
                    return NotFound();

                if (countryId != null)
                {
                    obj = obj.Where(x => x.CountryId == countryId).ToList();
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostCountryRegulationGroupMapping")]
        public async Task<ActionResult> PostCountryRegulationGroupMapping([FromBody] RegulationGroupModel regulation)
        {
            try
            {
                var obj = await _regulationGroup.PostCountryRegulationGroupMapping(regulation);
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
        [Route("GetRegulationGroupApprovalList/{UserUID}")]
        public async Task<ActionResult> GetRegulationGroupApprovalList(Guid UserUID)
        {
            try
            {
                var obj = await _regulationGroup.GetRegulationGroupApprovalList(UserUID);
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
        [Route("GetAllRegulationGroupApprovalList")]
        public async Task<ActionResult> GetAllRegulationGroupApprovalList()
        {
            try
            {
                var obj = await _regulationGroup.GetAllRegulationGroupApprovalList();
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
        [Route("GetAllCountryRegulationGroupMappingApproval")]
        public async Task<ActionResult> GetCountryRegulationGroupMappingApproval()
        {
            try
            {
                var obj = await _regulationGroup.GetAllCountryRegulationGroupMappingApproval();
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
        [Route("GetCountryRegulationGroupMappingApproval/{UserUID}")]
        public async Task<ActionResult> GetCountryRegulationGroupMappingApproval(Guid UserUID)
        {
            try
            {
                var obj = await _regulationGroup.GetCountryRegulationGroupMappingApproval(UserUID);
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
        [Route("PostRegulationGroupApprove")]
        public async Task<ActionResult> PostRegulationGroupApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationGroup.PostRegulationGroupApprove(access);
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
        [Route("PostRegulationGroupForward")]
        public async Task<ActionResult> PostRegulationGroupForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationGroup.PostRegulationGroupForward(access);
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
        [Route("PostRegulationGroupReject")]
        public async Task<ActionResult> PostRegulationGroupReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationGroup.PostRegulationGroupReject(access);
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
        [Route("PostCountryRegulationGroupApprove")]
        public async Task<ActionResult> PostCountryRegulationGroupMappingApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationGroup.PostCountryRegulationGroupMappingApprove(access);
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
        [Route("PostCountryRegulationGroupReject")]
        public async Task<ActionResult> PostCountryRegulationGroupMappingReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationGroup.PostCountryRegulationGroupMappingReject(access);
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
        [Route("PostCountryRegulationGroupForward")]
        public async Task<ActionResult> PostCountryRegulationGroupForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _regulationGroup.PostCountryRegulationGroupMappingForward(access);
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
        [Route("GetNextRegulationGroupCode")]
        public async Task<ActionResult> GetNextRegulationGroupCode()
        {
            try
            {
                var code = await _regulationGroup.GetNextRegulationGroupCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}