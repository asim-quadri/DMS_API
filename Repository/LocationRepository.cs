using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using ComplianceAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Repository
{
    public interface ILocationRepository
    {
       Task<List<EntityLocationModel>> GetClientEntitiesLocations();
    }
    public class LocationRepository : ILocationRepository
    {
        private readonly ComplianceDbContext _dbContext;
        private readonly IGetCoordinates _getCoordinates;
        private readonly IBillingService _billingService;

        public LocationRepository(ComplianceDbContext dbContext, IGetCoordinates getCoordinates, IBillingService billingService)
        {
            _dbContext = dbContext;
            _getCoordinates = getCoordinates;
            _billingService = billingService;
        }

        public async Task<List<EntityLocationModel>> GetClientEntitiesLocations()
        {
            var entities = await _dbContext.Entity
                .ToListAsync();

            var orgIds = entities.Select(e => e.OrganizationId).Distinct().ToList();
            var organizations = await _dbContext.Organizations
                .Where(o => orgIds.Contains(o.Id))
                .ToDictionaryAsync(o => o.Id, o => o.OrganizationName);

            var entityIds = entities.Select(e => e.Id).ToList();

            // Fetch all billing details for all entities in one query
            var allBillingDetails = await _dbContext.BillingDetails
                .Where(b => b.EntityId.HasValue && entityIds.Contains(b.EntityId.Value))
                .ToListAsync();

            // Fetch all bill status names
            var billStatusIds = allBillingDetails
                .Where(b => b.BillStatus.HasValue)
                .Select(b => b.BillStatus.Value)
                .Distinct()
                .ToList();

            var billStatuses = await _dbContext.BillStatus
                .Where(bs => billStatusIds.Contains(bs.Id))
                .ToDictionaryAsync(bs => bs.Id, bs => bs.BillStatusName);

            // Fetch all service requests for all entities in one query
            var allServiceRequests = await _dbContext.ServiceRequests
                .Where(sr => entityIds.Contains(sr.EntityId))
                .ToListAsync();

            // Pre-fetch coordinates for all cities to minimize repeated lookups
            var cityNames = entities.Select(e => e.City?.Trim()).Where(c => !string.IsNullOrEmpty(c)).Distinct().ToList();
            var cityCoordinates = new Dictionary<string, EntitiesCityCoordinate>();
            foreach (var city in cityNames)
            {
                var coord = await _getCoordinates.GetCoordinatesFromCityAsync(city);
                if (coord != null)
                    cityCoordinates[city] = coord;
            }

            // Fetch country names and state names for all entities
            var countryIds = entities.Where(e => e.CountryId.HasValue).Select(e => e.CountryId.Value).Distinct().ToList();
            var stateIds = entities.Where(e => e.StateId.HasValue).Select(e => e.StateId.Value).Distinct().ToList();

            var countries = await _dbContext.Country
                .Where(c => countryIds.Contains((long)c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.CountryName);

            var states = await _dbContext.States
                .Where(s => stateIds.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, s => s.StateName);

            var result = new List<EntityLocationModel>();
            var now = DateTime.Now.Date;
            var nowPlus7 = now.AddDays(7);

            foreach (var entity in entities)
            {
                var coordinates = cityCoordinates.TryGetValue(entity.City?.Trim(), out var coord)
                    ? coord
                    : null;

                // Filter billing details for this entity
                var billingDetails = allBillingDetails
                    .Where(b => b.EntityId == entity.Id)
                    .ToList();

                int billingRed = 0, billingAmber = 0, billingGreen = 0;

                foreach (var b in billingDetails)
                {
                    var billStatusName = b.BillStatus.HasValue && billStatuses.TryGetValue(b.BillStatus.Value, out var statusName) ? statusName : null;

                    string colorName = "Green";
                    if (string.Equals(billStatusName, "Received", StringComparison.OrdinalIgnoreCase))
                    {
                        colorName = "Green";
                    }
                    else if (b.DueDate.HasValue)
                    {
                        var dueDate = b.DueDate.Value.Date;
                        if (dueDate < now)
                            colorName = "Red";
                        else if (dueDate <= nowPlus7)
                            colorName = "Amber";
                        else
                            colorName = "Green";
                    }

                    if (colorName == "Red") billingRed++;
                    else if (colorName == "Amber") billingAmber++;
                    else if (colorName == "Green") billingGreen++;
                }

                // Filter service requests for this entity
                var serviceRequests = allServiceRequests
                    .Where(sr => sr.EntityId == entity.Id)
                    .ToList();

                int srRed = 0, srAmber = 0, srGreen = 0;
                foreach (var sr in serviceRequests)
                {
                    string statusDisplay = Enum.IsDefined(typeof(LevelType), (int)sr.LevelMasterId)
                        ? Enum.GetName(typeof(LevelType), (int)sr.LevelMasterId)
                        : sr.LevelMasterId.ToString();

                    string colorName = "Green";
                    if (string.Equals(statusDisplay, "Red", StringComparison.OrdinalIgnoreCase))
                        colorName = "Red";
                    else if (string.Equals(statusDisplay, "Amber", StringComparison.OrdinalIgnoreCase))
                        colorName = "Amber";
                    else if (string.Equals(statusDisplay, "Green", StringComparison.OrdinalIgnoreCase))
                        colorName = "Green";

                    if (colorName == "Red") srRed++;
                    else if (colorName == "Amber") srAmber++;
                    else if (colorName == "Green") srGreen++;
                }

                // Determine the maximum count color for billing details
                string billingMaxColor = "Amber";
                int billingMaxCode = 1;
                if (billingDetails.Count > 0)
                {
                    if (billingRed >= billingAmber && billingRed >= billingGreen)
                    {
                        billingMaxColor = "Red";
                        billingMaxCode = 2;
                    }
                    else if (billingAmber >= billingRed && billingAmber >= billingGreen)
                    {
                        billingMaxColor = "Amber";
                        billingMaxCode = 1;
                    }
                    else
                    {
                        billingMaxColor = "Green";
                        billingMaxCode = 3;
                    }
                }

                // Determine the maximum count color for service requests
                string srMaxColor = "Amber";
                int srMaxCode = 1;
                if (serviceRequests.Count > 0)
                {
                    if (srRed >= srAmber && srRed >= srGreen)
                    {
                        srMaxColor = "Red";
                        srMaxCode = 2;
                    }
                    else if (srAmber >= srRed && srAmber >= srGreen)
                    {
                        srMaxColor = "Amber";
                        srMaxCode = 1;
                    }
                    else
                    {
                        srMaxColor = "Green";
                        srMaxCode = 3;
                    }
                }

                result.Add(new EntityLocationModel
                {
                    Id = entity.Id,
                    EntityName = entity.EntityName,
                    OrganizationId = entity.OrganizationId,
                    OrganizationName = entity.OrganizationId.HasValue && organizations.ContainsKey(entity.OrganizationId.Value)
                        ? organizations[entity.OrganizationId.Value]
                        : string.Empty,
                    CountryId = entity.CountryId?.ToString(),
                    CountryName = entity.CountryId.HasValue && countries.TryGetValue((int?)entity.CountryId.Value, out var countryName) ? countryName : null,
                    StateId = entity.StateId,
                    StateName = entity.StateId.HasValue && states.TryGetValue(entity.StateId.Value, out var stateName) ? stateName : null,
                    City = entity.City,
                    Address = entity.Address,
                    Pin = entity.Pin,
                    Latitude = coordinates?.Latitude ?? 0,
                    Longitude = coordinates?.Longitude ?? 0,
                    BillingDetailsMaxColorCode = billingMaxCode,
                    BillingDetailsMaxColorName = billingMaxColor,
                    ServiceRequestMaxColorCode = srMaxCode,
                    ServiceRequestMaxColorName = srMaxColor
                });
            }

            return result;
        }
    }
}
