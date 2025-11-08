using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAccessServices _accessServices;

        public AccessController(IAccessServices accessServices)
        {
            _accessServices = accessServices;
        }

        [HttpGet]
        [Route("GetAccess/{UserUID}")]
        public async Task<ActionResult> GetAccess(Guid? UserUID)
        {
            try
            {
                var obj = await _accessServices.GetAccessList(UserUID);
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
        [Route("GetAllProductApproval/{UserUID}")]
        public async Task<ActionResult> GetAllProductApproval(Guid? UserUID)
        {
            try
            {
                var obj = await _accessServices.GetPendingApproval(UserUID);
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
        [Route("PostUserManagement")]
        public async Task<ActionResult> PostUserManagement([FromBody] List<AccessModel> access)
        {
            try
            {
                var obj = await _accessServices.PostUserManagement(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostApproveAccess")]
        public async Task<ActionResult> PostApproveAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _accessServices.PostApproveAccess(access);
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
                var obj = await _accessServices.PostRejectAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMenuOptions")]
        public async Task<ActionResult> GetMenuOptions(int roleId, int userId)
        {
            try
            {
                var obj = await _accessServices.GetMenuOptions(roleId, userId);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserAccess")]
        public async Task<ActionResult> GetUserAccess(int userId)
        {
            try
            {
                var obj = await _accessServices.GetUserAccess(userId);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SetUserAccess")]
        public async Task<ActionResult> SetUserAccess([FromBody] List<SetAccessRequest> accessRequests)
        {
            try
            {
                if (accessRequests == null || !accessRequests.Any())
                {
                    return BadRequest("User access list cannot be null or empty.");
                }
                var result = await _accessServices.SetUserAccess(accessRequests);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMenuOptionsForParent")]
        public async Task<ActionResult> GetMenuOptionsForParent(int parentMenuId, int userId)
        {
            try
            {
                var result = await _accessServices.GetMenuOptionsForParent(parentMenuId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddUserOrganization")]
        public async Task<ActionResult> AddUserOrganization([FromBody] List<UsersOrganizations> models)
        {
            try
            {
                if (models == null || !models.Any())
                {
                    return BadRequest("Input list cannot be null or empty.");
                }
                var result = await _accessServices.PostUserOrganizationMapping(models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddUserRegulationMapping")]
        public async Task<IActionResult> AddUserRegulationMapping([FromBody] PostUserRegulationMapping dto)
        {
            try
            {
                var result = await _accessServices.AddUserRegulationMappingAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserRegulationMappingDetails/{userId}")]
        public async Task<IActionResult> GetUserRegulationMappingDetails(long userId)
        {
            try
            {
                var result = await _accessServices.GetUserRegulationMappingDetailsByUserIdAsync(userId);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while retrieving user regulation mapping details.");
            }
        }
    }
}