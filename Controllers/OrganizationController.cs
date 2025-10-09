using Dms_Api.Models;
using Dms_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dms_Api.Controllers
{
    // Controllers/OrganisationController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganisationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] Organization organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _organizationService.AddOrganization(organization);
            return new JsonResult(result);

        }

    

        [HttpGet]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var organisations = await _organizationService.GetAllOrganizations();
            return Ok(organisations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            var result = await _organizationService.DeleteOrganization(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}
