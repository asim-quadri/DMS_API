using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers.ProductOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcernedMinistryController : ControllerBase
    {
        private readonly IConcernedMinistryService _service;
        private readonly ILogger<ConcernedMinistryController> _logger;

        public ConcernedMinistryController(IConcernedMinistryService service, ILogger<ConcernedMinistryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("PostConcernedMinistry")]
        public async Task<ActionResult<Result>> PostConcernedMinistry([FromBody] PostConcernedMinistry postConcernedMinistry)
        {
            try
            {
                var result = await _service.PostConcernedMinistry(postConcernedMinistry);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while posting concerned ministry.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetNextConcernedMinistryRefCode")]
        public async Task<ActionResult<string>> GetNextConcernedMinistryRefCode()
        {
            try
            {
                var code = await _service.GetNextConcernedMinistryRefCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting next concerned ministry reference code.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetAllConcernedMinistry")]
        public async Task<ActionResult<List<ConcernedMinistry>>> GetAllConcernedMinistry()
        {
            try
            {
                var ministries = await _service.GetAllConcernedMinistry();
                return Ok(ministries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all concerned ministries.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetAllPendingConcernedMinistry/{id}")]
        public async Task<ActionResult<List<PostConcernedMinistry>>> GetAllPendingConcernedMinistry(long id)
        {
            try
            {
                var pending = await _service.GetAllPendingConcernedMinistry(id);
                return Ok(pending);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all pending concerned ministries.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("SubmitConcernedMinistriesApprove")]
        public async Task<ActionResult<Result>> SubmitConcernedMinistriesApprove([FromBody] PostConcernedMinistry postConcernedMinistry)
        {
            try
            {
                var result = await _service.SubmitConcernedMinistriesApprove(postConcernedMinistry);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting concerned ministries approve.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("PostCountryConcernedMinistryMapping")]
        public async Task<ActionResult<Result>> PostCountryConcernedMinistryMapping([FromBody] CountryConcernedMinistryMapping countryConcernedMinistry)
        {
            try
            {
                var result = await _service.PostCountryConcernedMinistryMapping(countryConcernedMinistry);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while posting country concerned ministry mapping.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetAllPendingConcernedMinistryMapping/{id}")]
        public async Task<ActionResult<List<CountryConcernedMinistryMapping>>> GetAllPendingConcernedMinistryMapping(long id)
        {
            try
            {
                var pending = await _service.GetAllPendingConcernedMinistryMapping(id);
                return Ok(pending);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all pending concerned ministry mappings.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("SubmitConcernedMinistriesMappingApprove")]
        public async Task<ActionResult<Result>> SubmitConcernedMinistriesMappingApprove([FromBody] CountryConcernedMinistryMapping countryConcernedMinistry)
        {
            try
            {
                var result = await _service.SubmitConcernedMinistriesMappingApprove(countryConcernedMinistry);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting concerned ministries mapping approve.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetAllConcernedMinistryMapping")]
        public async Task<ActionResult<List<CountryConcernedMinistryMapping>>> GetAllConcernedMinistryMapping()
        {
            try
            {
                var mappings = await _service.GetAllConcernedMinistryMapping();
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all concerned ministry mappings.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("GetConcernedMinistryListCountry/{countryId:int}")]
        public async Task<ActionResult<List<ConcernedMinistry>>> GetConcernedMinistryListCountry(int countryId)
        {
            try
            {
                var concernedMinistries = await _service.GetConcernedMinistryListCountry(countryId);
                return Ok(concernedMinistries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetConcernedMinistryListCountry:-Error occurred while getting all concerned ministry based on country id.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}