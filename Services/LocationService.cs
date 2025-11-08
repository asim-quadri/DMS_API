using ComplianceAPI.Models;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Services
{
    public interface ILocationService
    {
      Task<List<EntityLocationModel>> GetClientEntitiesLocations();
    }
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public Task<List<EntityLocationModel>> GetClientEntitiesLocations()
        {
            return _locationRepository.GetClientEntitiesLocations();
        }
    }
}
