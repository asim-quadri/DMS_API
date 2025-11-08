using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _entityService;

        public EntityController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [HttpGet]
        [Route("GetEntityApprovalList/{userId}")]
        public async Task<ActionResult> GetEntityApprovalList(int userId)
        {
            try
            {
                var obj = await _entityService.GetEntityApprovalList(userId);
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
        [Route("GetAllEntitiesByOrgId")]
        public async Task<ActionResult> GetAllEntitiesByOrgId([FromQuery] int orgId)
        {
            try
            {
                var obj = await _entityService.GetAllEntitiesByOrgId(orgId);
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
        [Route("GetEntityDetails")]
        public async Task<ActionResult> GetEntityDetails([FromQuery] int entityId)
        {
            try
            {
                var obj = await _entityService.GetEntityDetails(entityId);
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
        [Route("GetEntityView")]
        public async Task<ActionResult> GetEntityView([FromQuery] int entityId)
        {
            try
            {
                var obj = await _entityService.GetEntityView(entityId);
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
        [Route("PostEntity")]
        public async Task<ActionResult> PostEntity([FromBody] PostEntity entity)
        {
            try
            {
                var obj = await _entityService.PostEntity(entity);
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
        [Route("PostEntityApprove")]
        public async Task<ActionResult> PostEntityApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _entityService.PostEntityApprove(access);
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
        [Route("PostEntityReject")]
        public async Task<ActionResult> PostEntityReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _entityService.PostEntityReject(access);
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
        [Route("PostEntityForward")]
        public async Task<ActionResult> PostEntityForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _entityService.PostEntityForward(access);
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
        [Route("GetStartAndEndMonthsByCountryId")]
        public async Task<ActionResult> GetStartAndEndMonthsByCountryId([FromQuery] int countryId)
        {
            try
            {
                var obj = await _entityService.GetStartAndEndMonthsByCountryId(countryId);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEntitiesByOrganizationId/{organizationId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEntitiesByOrganizationId(long organizationId)
        {
            try
            {
                var response = await _entityService.GetEntitiesByOrganizationId(organizationId);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetEntitiesByOrganizationAndCountryId/{organizationId}/{countryId}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEntitiesByOrganizationAndCountryId(long organizationId,long countryId)
        {
            try
            {
                var response = await _entityService.GetEntitiesByOrganizationAndCountryId(organizationId,countryId);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpGet]
        [Route("GetEntitiesLocations/{organizationId}")]
        public async Task<ActionResult> GetClientEntitiesLocations(int organizationId)
        {
            try
            {
                var obj = await _entityService.GetClientEntitiesLocations(organizationId);
                if (obj != null)
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