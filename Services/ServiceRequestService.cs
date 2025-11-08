using ComplianceAPI.Models;
using ComplianceAPI.Models.Enums;
using ComplianceAPI.Repository;
using Dto = ComplianceAPI.Models;

namespace ComplianceAPI.Services
{
    public interface IServiceRequestService
    {
        /// <summary>
        /// Updates the expected date for a specified service request.
        /// </summary>
        /// <param name="serviceRequestId">The unique identifier of the service request to update.</param>
        /// <param name="expectedDate">The new expected date to set for the service request.</param>
        /// <param name="levelMasterId">The identifier of the level master associated with the update.</param>
        /// <returns>A <see cref="Result{T}"/><see langword="true"/> if the expected date was successfully updated; otherwise, <see langword="false"/>.</returns>
        Task<Result<bool>> SetExpectedDateAndLevelTypeAsync(Dto.ServiceRequest serviceRequest);

        Task<List<Dto.ServiceRequest>> GetServiceRequestsAsync(
          ServiceRequestSortBy sortBy);

        Task<List<LevelMaster>> GetAllLevelAsync();

        Task<List<ServiceRequestDetails>> GetServiceRequestsByEntityAsync(long entityId);

    }

    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly ILogger<ServiceRequestService> _logger;

        public ServiceRequestService(IServiceRequestRepository serviceRequestRepository, ILogger<ServiceRequestService> logger)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _logger = logger;
        }

        public async Task<List<Dto.ServiceRequest>> GetServiceRequestsAsync(ServiceRequestSortBy sortBy)
        {
            var response = await _serviceRequestRepository.GetServiceRequestsAsync(sortBy);
            return response;
        }

        public async Task<Result<bool>> SetExpectedDateAndLevelTypeAsync(Dto.ServiceRequest serviceRequest)
        {
            var result = new Result<bool>();
            try
            {
                _logger.LogInformation("Setting expected date for ServiceRequest with Id {ServiceRequestId} and LevelMasterId {LevelMasterId}", serviceRequest.Id, serviceRequest.LevelMasterId);
                result = await _serviceRequestRepository.SetExpectedDateAsync(serviceRequest);
                if (result.Success)
                {
                    _logger.LogInformation("Successfully updated expected date for ServiceRequest with Id {ServiceRequestId}", serviceRequest.Id);
                }
                else
                {
                    _logger.LogError("Failed to update expected date for ServiceRequest with Id {ServiceRequestId}: {Message}", serviceRequest.Id, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting expected date for ServiceRequest with Id {ServiceRequestId}", serviceRequest.Id);
                result.Success = false;
                result.Message = $"An error occurred while setting expected date: {ex.Message}";
            }
            return result;
        }

        public async Task<List<LevelMaster>> GetAllLevelAsync()
        {
            var response = new List<LevelMaster>();
            try
            {
                var levels = await _serviceRequestRepository.GetAllLevelAsync();
                if (levels == null || !levels.Any())
                {
                    _logger.LogWarning("GetAllLevel: No levels found in the database.");
                    return response;
                }
                response = levels.Select(l => new LevelMaster
                {
                    Id = l.Id,
                    LevelType = l.LevelType,
                    IsActive = l.IsActive,
                    CreatedOn = l.CreatedOn
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllLevel: Error occurred while retrieving levels.");
            }
            return response;
        }

        public async Task<List<ServiceRequestDetails>> GetServiceRequestsByEntityAsync(long entityId)
        {
            return await _serviceRequestRepository.GetServiceRequestsByEntityAsync(entityId);
        }
    }
}