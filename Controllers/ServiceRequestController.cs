using ComplianceAPI.Models;
using ComplianceAPI.Models.Enums;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequestService;
        private readonly ILogger<ServiceRequestController> _logger;

        public ServiceRequestController(IServiceRequestService serviceRequestService, ILogger<ServiceRequestController> logger)

        {
            _serviceRequestService = serviceRequestService;
            _logger = logger;
        }

        /// <summary>
        /// Updates the expected date and level type for a specified service request.
        /// </summary>
        /// <param name="serviceRequestId">The unique identifier of the service request to update.</param>
        /// <param name="expectedDate">The new expected date to set for the service request.</param>
        /// <param name="levelMasterId">The identifier of the level master associated with the update.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [HttpPost("SetExpectedDateAndLevelTypeAsync")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetExpectedDateAndLevelTypeAsync(ServiceRequest serviceRequest)
        {
            try
            {
                var result = await _serviceRequestService.SetExpectedDateAndLevelTypeAsync(serviceRequest);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting expected date and level type for ServiceRequestId: {ServiceRequestId}", serviceRequest.Id);
                var errorResult = new Result<bool>
                {
                    Success = false,
                    Message = $"An error occurred while setting expected date and level type: {ex.Message}"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResult);
            }
        }

        /// <summary>
        /// Retrieves a list of service requests with optional sorting.
        /// </summary>
        /// <param name="sortBy">The field to sort by (default: Recent).</param>
        /// <param name="sortDirection">The sort direction ("asc" or "desc", default: "desc").</param>
        /// <returns>A list of service requests.</returns>
        [HttpGet("GetServiceRequestsAsync")]
        [ProducesResponseType(typeof(Result<List<ServiceRequest>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetServiceRequestsAsync(
            [FromQuery] ServiceRequestSortBy sortBy)
        {
            try
            {
                var serviceRequests = await _serviceRequestService.GetServiceRequestsAsync(sortBy);

                return Ok(serviceRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving service requests.");
                var errorResult = new Result<List<ServiceRequest>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving service requests: {ex.Message}"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResult);
            }
        }

        [HttpGet]
        [Route("GetAllLevel")]
        [ProducesResponseType(typeof(List<LevelMaster>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<LevelMaster>>> GetAllLevel()
        {
            try
            {
                var levels = await _serviceRequestService.GetAllLevelAsync();
                if (levels != null && levels.Any())
                    return Ok(levels);
                return NotFound("No levels found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllLevel:-Error occurred while retrieving all levels.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetServiceRequestsByEntityAsync")]
        [ProducesResponseType(typeof(List<ServiceRequest>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetServiceRequestsByEntityAsync([FromQuery] long entityId)
        {
            try
            {
                var serviceRequests = await _serviceRequestService.GetServiceRequestsByEntityAsync(entityId);
                if (serviceRequests != null && serviceRequests.Any())
                {
                    return Ok(serviceRequests);
                }
                return NotFound("No service requests found for the given organization and entity.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching service requests for EntityId: {EntityId}",  entityId);
                return BadRequest(ex.Message);
            }
        }
    }
}