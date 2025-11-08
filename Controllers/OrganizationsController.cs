using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationServices _organizationServices;

        public OrganizationsController(IOrganizationServices organizationServices)
        {
            _organizationServices = organizationServices;
        }

        [HttpGet]
        [Route("GetAllBillingLevel")]
        public async Task<ActionResult> GetAllBillingLevel()
        {
            try
            {
                var obj = await _organizationServices.GetAllBillingLevel();
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
        [Route("GetAllProductType")]
        public async Task<ActionResult> GetAllProductType()
        {
            try
            {
                var obj = await _organizationServices.GetAllProductType();
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
        [Route("GetAllOrganization/{userId}")]
        public async Task<ActionResult> GetAllOrganization(int? userId)
        {
            try
            {
                var obj = await _organizationServices.GetAllOrganization(userId);
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
        [Route("GetAllOrganizations")]
        public async Task<ActionResult> GetAllOrganizations()
        {
            try
            {
                var obj = await _organizationServices.GetAllOrganizations();
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
        [Route("GetOrganizationById/{Id}")]
        public async Task<ActionResult> GetOrganizationById(long Id)
        {
            try
            {
                var obj = await _organizationServices.GetOrganizationById(Id);
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
        [Route("GetAllOrganizationWithoutStatus")]
        public async Task<ActionResult> GetAllOrganizationWithoutStatus([FromQuery] int? user = null)
        {
            try
            {
                var obj = await _organizationServices.GetAllOrganizationWithoutStatus(user);
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
        [Route("GetOrganizationApprovalList/{userUID}")]
        public async Task<ActionResult> GetOrganizationApprovalList(string userUID)
        {
            try
            {
                var obj = await _organizationServices.GetOrganizationApprovalList(userUID);
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
        [Route("GetBillingLevelById")]
        public async Task<ActionResult> GetBillingLevelById(int id)
        {
            try
            {
                var obj = await _organizationServices.GetBillingLevelById(id);
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
        [Route("PostOrganization")]
        public async Task<ActionResult> PostOrganization([FromBody] PostOrganization organization)
        {
            try
            {
                var obj = await _organizationServices.PostOrganization(organization);
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
        [Route("PostOrganizationApprove")]
        public async Task<ActionResult> PostOrganizationApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _organizationServices.PostOrganizationApprove(access);
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
        [Route("PostOrganizationReject")]
        public async Task<ActionResult> PostOrganizationReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _organizationServices.PostOrganizationReject(access);
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
        [Route("PostOrganizationForward")]
        public async Task<ActionResult> PostOrganizationForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _organizationServices.PostOrganizationForward(access);
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
        [Route("GetOrgEntityList/{userId}")]
        public async Task<ActionResult> GetOrgEntityList(int? userId)
        {
            try
            {
                var obj = await _organizationServices.GetOrgEntityList(userId);
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
        [Route("GetOrgEntityLists")]
        public async Task<ActionResult> GetOrgEntityLists()
        {
            try
            {
                var obj = await _organizationServices.GetOrgEntityLists();
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserOrganizations/{userId}")]
        [ProducesResponseType(typeof(List<OrganizationDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<OrganizationDetail>>> GetUserOrganizations(long userId)
        {
            try
            {
                var response = await _organizationServices.GetUserOrganizationsByUserId(userId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new List<OrganizationDetail>());
            }
        }

        [HttpGet("GetOrganizationsByCountry/{countryId}")]
        public async Task<ActionResult> GetOrganizationsByCountry(long countryId)
        {
            try
            {
                var result = await _organizationServices.GetOrganizationsByCountryId(countryId);
                if (result != null && result.Any())
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrganizationsByUserandCountry/{userId}/{countryId}")]
        public async Task<ActionResult> GetOrganizationsByUserandCountry(long userId,long countryId)
        {
            try
            {
                var result = await _organizationServices.GetOrganizationsByUserandCountry(userId,countryId);
                if (result != null && result.Any())
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}