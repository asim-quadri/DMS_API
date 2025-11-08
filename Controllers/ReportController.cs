using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly Services.IReportService _reportService;

        public ReportController(ILogger<ReportController> logger, Services.IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        [HttpGet("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                var countries = await _reportService.GetAllCountriesAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching countries.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllReportMaster")]
        public async Task<IActionResult> GetAllReportMaster()
        {
            try
            {
                var reportMasters = await _reportService.GetAllReportMaster();
                return Ok(reportMasters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching report masters.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllRegulationWithCountries")]
        public async Task<IActionResult> GetAllRegulationWithCountriesAsync()
        {
            try
            {
                var regulations = await _reportService.GetAllRegulationWithCountriesAsync();
                return Ok(regulations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching regulations with countries.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetAllComplianceWithCountriesReports")]
        public async Task<IActionResult> GetAllComplianceWithCountriesReports()
        {
            try
            {
                var regulations = await _reportService.GetAllComplianceWithCountryAsync();
                return Ok(regulations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching compliance with countries reports.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetIndustryMappingWithCountry")]
        public async Task<IActionResult> GetIndustryMapping()
        {
            try
            {
                var industryMappings = await _reportService.GetIndustryMapping();
                return Ok(industryMappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching industry mappings.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllEntityTypeWithCountry")]
        public async Task<IActionResult> GetAllEntityTypes()
        {
            try
            {
                var entityMappings = await _reportService.GetAllEntityTypes();
                return Ok(entityMappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching EntityTypes mappings.");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}