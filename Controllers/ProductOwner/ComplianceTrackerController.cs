using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers.ProductOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplianceTrackerController : ControllerBase
    {
        private readonly IComplianceTrackerService _complianceTrackerService;

        public ComplianceTrackerController(IComplianceTrackerService complianceTrackerService)
        {
            _complianceTrackerService = complianceTrackerService;
        }

        [HttpGet]
        [Route("GetAllRegulationGroups")]
        public async Task<ActionResult> GetAllRegulationGroups()
        {
            try
            {
                var obj = await _complianceTrackerService.GetAllRegulationGroups();
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
        [Route("GetTOCListByComplianceId/{ComplianceId}")]
        public async Task<ActionResult> GetTOCListByComplianceId(long ComplianceId)
        {
            try
            {
                var obj = await _complianceTrackerService.GetTOCList(ComplianceId);
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
        [Route("GetTOCListByRegulationId/{RegulationId}")]
        public async Task<ActionResult> GetTOCListByRegulationId(long RegulationId)
        {
            try
            {
                var obj = await _complianceTrackerService.GetRegulationTOCList(RegulationId);
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
        [Route("GetRegComplianceDetails")]
        public async Task<ActionResult> GetRegComplianceDetails()
        {
            try
            {
                var obj = await _complianceTrackerService.GetRegComplianceDetails();
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
        [Route("GetRegComplianceDetailsByEntityId/{EntityId}")]
        public async Task<ActionResult> GetRegComplianceDetailsByEntityId(long EntityId)
        {
            try
            {
                var obj = await _complianceTrackerService.GetRegComplianceDetailsByEntityId(EntityId);
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
        [Route("GetEntities")]
        public async Task<ActionResult> GetEntities()
        {
            try
            {
                var obj = await _complianceTrackerService.GetEntities();
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
        [Route("GetEntityById/{EntityId}")]
        public async Task<ActionResult> GetEntityById(long EntityId)
        {
            try
            {
                var obj = await _complianceTrackerService.GetEntityById(EntityId);
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
        [Route("PostComplianceTracker")]
        public async Task<ActionResult> PostComplianceTracker([FromBody] ComplianceTracker compliance)
        {
            try
            {
                var obj = await _complianceTrackerService.PostComplianceTracker(compliance);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllCompliances")]
        public async Task<ActionResult> GetAllCompliances()
        {
            try
            {
                var obj = await _complianceTrackerService.GetAllCompliances();
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCompliancesByRegulationSetupId")]
        public async Task<ActionResult> GetCompliancesByRegulationSetupId(int regulationSetupId)
        {
            try
            {
                var obj = await _complianceTrackerService.GetAllCompliances();
                if (obj == null)
                {
                    return NotFound();
                }
                obj = obj.Where(c => c.RegulationSetupId == regulationSetupId).ToList();

                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}