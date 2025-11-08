using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers.ProductOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegulatoryAuthorityController : ControllerBase
    {
        private readonly ILogger<RegulatoryAuthorityController> _logger;
        private readonly IRegulatoryAuthorityService _regulatoryAuthorityService;

        public RegulatoryAuthorityController(ILogger<RegulatoryAuthorityController> logger, IRegulatoryAuthorityService regulatoryAuthorityService)
        {
            _logger = logger;
            _regulatoryAuthorityService = regulatoryAuthorityService;
        }

        [HttpPost]
        [Route("PostRegulatoryAuthority")]
        public async Task<ActionResult> PostRegulatoryAuthority(PostRegulatoryAuthorities regulatoryAuthorities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _regulatoryAuthorityService.PostRegulatoryAuthority(regulatoryAuthorities);

                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostRegulatoryAuthority:-Error occurred while posting regulatory authority.");
                return StatusCode(500, "PostRegulatoryAuthority:-An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetNextRegulatoryAuthRefCode")]
        public async Task<ActionResult<string>> GetNextRegulatoryAuthRefCode()
        {
            try
            {
                var nextRefCode = await _regulatoryAuthorityService.GetNextRegulatoryAuthRefCode();
                return Ok(nextRefCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetNextRegulatoryAuthRefCode:-Error occurred while retrieving next regulatory authority reference code.");
                return StatusCode(500, "GetNextRegulatoryAuthRefCode:-An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetAllRegulatoryAuthorities")]
        public async Task<ActionResult> GetAllRegulatoryAuthorities()
        {
            try
            {
                var authorities = await _regulatoryAuthorityService.GetAllRegulatoryAuthorities();
                return Ok(authorities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllRegulatoryAuthorities:-Error occurred while retrieving regulatory authorities.");
                return StatusCode(500, "GetAllRegulatoryAuthorities:-An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetAllPendingRegAuth/{id:long}")]
        public async Task<ActionResult<List<PostRegulatoryAuthorities>>> GetAllPendingRegAuth(long id)
        {
            try
            {
                var pendingAuthorities = await _regulatoryAuthorityService.GetAllPendingRegAuth(id);
                return Ok(pendingAuthorities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllPendingRegAuth:-Error occurred while retrieving pending regulatory authorities.");
                return StatusCode(500, "GetAllPendingRegAuth:-An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Route("SubmitRegulatoryAuthoritiesApprove")]
        public async Task<ActionResult> SubmitRegulatoryAuthoritiesApprove(PostRegulatoryAuthorities regulatoryAuthorities)
        {
            try
            {
                var result = await _regulatoryAuthorityService.SubmitRegulatoryAuthoritiesApprove(regulatoryAuthorities);

                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SubmitRegulatoryAuthoritiesApprove:-Error occurred while submitting regulatory authority approval.");
                return StatusCode(500, "SubmitRegulatoryAuthoritiesApprove:-An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Route("PostCountryRegulatoryAuthorityMapping")]
        public async Task<ActionResult> PostCountryRegulatoryAuthorityMapping([FromBody] ComplianceAPI.Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorityMapping)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _regulatoryAuthorityService.PostCountryRegulatoryAuthorityMapping(countryRegulatoryAuthorityMapping);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostCountryRegulatoryAuthorityMapping:-Error occurred while posting country regulatory authority mapping.");
                return StatusCode(500, "PostCountryRegulatoryAuthorityMapping:-An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetAllPendingRegAuthMapping/{id:long}")]
        public async Task<ActionResult<List<PostRegulatoryAuthorities>>> GetAllPendingRegAuthMapping(long id)
        {
            try
            {
                var pendingAuthorities = await _regulatoryAuthorityService.GetAllPendingRegAuthMapping(id);
                return Ok(pendingAuthorities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllPendingRegAuthMapping:-Error occurred while retrieving pending regulatory authorities.");
                return StatusCode(500, "GetAllPendingRegAuthMapping:-An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Route("SubmitRegulatoryAuthoritiesMappingApprove")]
        public async Task<ActionResult> SubmitRegulatoryAuthoritiesMappingApprove(ComplianceAPI.Models.CountryRegulatoryAuthorityMapping countryRegulatoryAuthorities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _regulatoryAuthorityService.SubmitRegulatoryAuthoritiesMappingApprove(countryRegulatoryAuthorities);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SubmitRegulatoryAuthoritiesMappingApprove:-Error occurred while submitting regulatory authority mapping approval.");
                return StatusCode(500, "SubmitRegulatoryAuthoritiesMappingApprove:-An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetAllRegAuthMapping")]
        public async Task<ActionResult<List<ComplianceAPI.Models.CountryRegulatoryAuthorityMapping>>> GetAllRegAuthMapping()
        {
            try
            {
                var mappings = await _regulatoryAuthorityService.GetAllRegAuthMapping();
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllRegAuthMapping:-Error occurred while retrieving regulatory authority mappings.");
                return StatusCode(500, "GetAllRegAuthMapping:-An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Route("GetRegulatoryAuthoritiesListCountry/{countryId:int}")]
        public async Task<ActionResult<List<ComplianceAPI.Models.RegulatoryAuthorities>>> GetRegulatoryAuthoritiesListCountry(int countryId)
        {
            try
            {
                var mappings = await _regulatoryAuthorityService.GetRegulatoryAuthoritiesListCountry(countryId);
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRegulatoryAuthoritiesListCountry:-Error occurred while retrieving regulatory authority mappings.");
                return StatusCode(500, "GetRegulatoryAuthoritiesListCountry:-An error occurred while processing your request.");
            }
        }
    }
}