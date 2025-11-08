using ComplianceAPI.Models;

namespace ComplianceAPI.Helpers
{
    public interface IGetCoordinates
    {    
        Task<EntitiesCityCoordinate?> GetCoordinatesFromCityAsync(string city);
    }
}
