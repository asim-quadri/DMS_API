using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

using Db = ComplianceAPI.Models.DataModels;
using Dto = ComplianceAPI.Models;

namespace ComplianceAPI.Repository
{
    public interface IServiceRequestRepository
    {
        /// <summary>
        /// Updates the expected date for a specified service request.
        /// </summary>
        /// <returns>A <see cref="Result{T}"/><see langword="true"/> if the expected date was successfully updated; otherwise, <see langword="false"/>.</returns>
        Task<Result<bool>> SetExpectedDateAsync(Dto.ServiceRequest requestData);

        Task<List<Dto.ServiceRequest>> GetServiceRequestsAsync(
            ServiceRequestSortBy sortBy = ServiceRequestSortBy.Recent);

        Task<List<Db.LevelMaster>> GetAllLevelAsync();
        Task<List<ServiceRequestDetails>> GetServiceRequestsByEntityAsync(long entityId);

    }

    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ComplianceDbContext _dbContext;
        private readonly ILogger<ServiceRequestRepository> _logger;
        private readonly IEmail _emailHelper;
        private readonly IConfiguration _configuration;

        public ServiceRequestRepository(ComplianceDbContext context, ILogger<ServiceRequestRepository> logger, IEmail emailHelper, IConfiguration configuration)
        {
            _dbContext = context;
            _logger = logger;
            _emailHelper = emailHelper;
            _configuration = configuration;
        }

        public async Task<Result<bool>> SetExpectedDateAsync(Dto.ServiceRequest requestData)
        {
            var result = new Result<bool>();
            try
            {
                _logger.LogInformation("Starting SetExpectedDateAsync for ServiceRequest Id: {Id}", requestData.Id);

                var serviceRequest = await _dbContext.ServiceRequests
                    .FirstOrDefaultAsync(sr => sr.Id == requestData.Id);

                if (serviceRequest == null)
                {
                    _logger.LogWarning("ServiceRequest with Id {Id} not found.", requestData.Id);
                    result.Message = $"ServiceRequest with Id {requestData.Id} not found.";
                    result.Data = false;
                    result.Success = false;
                    return result;
                }

                _logger.LogInformation("Validating LevelMasterId: {LevelMasterId}", requestData.LevelMasterId);
                var levelMasterExists = await _dbContext.LevelMasters
                    .AnyAsync(lm => lm.Id == requestData.LevelMasterId);
                if (!levelMasterExists)
                {
                    _logger.LogWarning("LevelMaster with Id {LevelMasterId} does not exist.", requestData.LevelMasterId);
                    result.Message = $"LevelMaster with Id {requestData.LevelMasterId} does not exist.";
                    result.Data = false;
                    result.Success = false;
                    return result;
                }

                bool expectedDateChanged = requestData.ExpectedDate != null && serviceRequest.ExpectedDate != requestData.ExpectedDate;
                bool levelMasterIdChanged = serviceRequest.LevelMasterId != requestData.LevelMasterId;
                bool commentsChanged = !string.Equals(serviceRequest.Comments, requestData.Comments, StringComparison.Ordinal);

                bool anyChange = false;
                List<string> updatedFields = new();

                if (expectedDateChanged)
                {
                    serviceRequest.ExpectedDate = requestData.ExpectedDate;
                    updatedFields.Add("Expected date");
                    anyChange = true;
                }
                if (levelMasterIdChanged)
                {
                    serviceRequest.LevelMasterId = requestData.LevelMasterId;
                    updatedFields.Add("LevelType");
                    anyChange = true;
                }
                if (commentsChanged)
                {
                    serviceRequest.Comments = requestData.Comments;
                    updatedFields.Add("Comments");
                    anyChange = true;
                }
                serviceRequest.ModifiedBy = requestData.ModifiedBy != null ? requestData.ModifiedBy : serviceRequest.ModifiedBy;
                if (requestData.ModifiedBy != null)
                {
                    updatedFields.Add("ModifiedBy");
                    anyChange = true;
                }

                serviceRequest.ModifiedOn = requestData.ModifiedOn != null ? requestData.ModifiedOn : serviceRequest.ModifiedOn;
                if (requestData.ModifiedOn != null)
                {
                    updatedFields.Add("ModifiedOn");
                    anyChange = true;
                }

                if (!anyChange)
                {
                    _logger.LogInformation("No changes detected for ServiceRequest Id: {Id}.", requestData.Id);
                    result.Message = "No changes detected. Nothing was updated.";
                    result.Data = false;
                    result.Success = false;
                    return result;
                }

                result.Message = $"{string.Join(", ", updatedFields)} updated successfully.";
                result.Success = true;
                result.Data = true;

                _logger.LogInformation("Updating ServiceRequest Id: {Id} with changes: {Changes}", requestData.Id, string.Join(", ", updatedFields));

                await _dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                   UPDATE [client].[ServiceRequest]
                   SET ExpectedDate = {requestData.ExpectedDate}, LevelMasterId = {requestData.LevelMasterId}, ModifiedBy={requestData.ModifiedBy}, ModifiedOn={requestData.ModifiedOn},Comments={requestData.Comments}
                   WHERE Id = {requestData.Id}");

                if (serviceRequest.UserId != 0)
                {
                    var clientInfo = await _dbContext.ClientUsers.FirstOrDefaultAsync(c => c.Id == serviceRequest.UserId);
                    if (clientInfo == null || clientInfo.Email == null)
                    {
                        result.Message = "Client user details not found";
                        return result;
                    }
                    var bestRegards = _configuration.GetValue<string>("EmailConfig:BestRegards") ?? "Team COMPSEQR360";
                    string htmlBody = string.Format(HtmlBody.RequestReceived, clientInfo.FullName, bestRegards);
                    var mailSent = await _emailHelper.SendEmailRequestProcess(clientInfo.Email, HtmlBody.RequestReceivedSubject, htmlBody);
                    result.Success = mailSent;
                    result.Message = mailSent ? "Email sent successfully" : "Email not sent";
                }
                _logger.LogInformation("Successfully updated ServiceRequest Id: {Id}.", requestData.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the expected date for ServiceRequest Id: {Id}.", requestData.Id);
                result.Success = false;
                result.Message = $"An error occurred while updating the expected date: {ex.Message}";
                result.Data = false;
                return result;
            }
        }

        public async Task<List<Dto.ServiceRequest>> GetServiceRequestsAsync(ServiceRequestSortBy sortBy = ServiceRequestSortBy.Recent)
        {
            try
            {
                _logger.LogInformation("Starting GetServiceRequestsAsync with sortBy: {SortBy}", sortBy);

                IQueryable<Db.ServiceRequest> allServiceRequests = _dbContext.ServiceRequests;

                if (sortBy == ServiceRequestSortBy.Red ||
                    sortBy == ServiceRequestSortBy.Amber ||
                    sortBy == ServiceRequestSortBy.Green)
                {
                    string color = sortBy.ToString();
                    _logger.LogInformation("Filtering ServiceRequests by LevelType: {LevelType}", color);
                    allServiceRequests = from sr in _dbContext.ServiceRequests
                                         join lm in _dbContext.LevelMasters on sr.LevelMasterId equals lm.Id
                                         where lm.LevelType == color
                                         select sr;
                }

                _logger.LogInformation("Building query for ServiceRequests.");
                var query = from sr in allServiceRequests
                            join org in _dbContext.Organizations on sr.OrganizationId equals org.Id into orgJoin
                            from org in orgJoin.DefaultIfEmpty()
                            join entity in _dbContext.Entity on sr.EntityId equals entity.Id into entityJoin
                            from entity in entityJoin.DefaultIfEmpty()
                            join lm in _dbContext.LevelMasters on sr.LevelMasterId equals lm.Id into lmJoin
                            from lm in lmJoin.DefaultIfEmpty()
                            join major in _dbContext.MajorModules on sr.MajorModuleId equals major.Id into majorJoin
                            from major in majorJoin.DefaultIfEmpty()
                            join minor in _dbContext.MinorModules on sr.MinorModuleId equals minor.Id into minorJoin
                            from minor in minorJoin.DefaultIfEmpty()
                            join user in _dbContext.Users on sr.UserId equals user.Id into userJoin
                            from user in userJoin.DefaultIfEmpty()
                            select new Dto.ServiceRequest
                            {
                                Id = sr.Id,
                                UserId = sr.UserId,
                                Status = sr.Status,
                                MajorModuleId = sr.MajorModuleId,
                                MajorModuleName = major.MajorModuleName,
                                MinorModuleId = sr.MinorModuleId,
                                MinorModuleName = minor.MinorModuleName,
                                LevelMasterId = sr.LevelMasterId,
                                CreatedOn = sr.CreatedOn,
                                CreatedDate = sr.CreatedOn.ToString("dd.MMM.yy"),
                                ExpDate = sr.ExpectedDate.HasValue ? sr.ExpectedDate!.Value.ToString("dd.MMM.yy") : null,
                                ExpectedDate = sr.ExpectedDate,
                                EntityName = entity.EntityName,
                                OrganizationId = sr.OrganizationId,
                                ApprovedStatus = RefApprovalStatusU.Approved,
                                OrganizationName = org != null ? org.OrganizationName : null,
                                LevelType = lm.LevelType,
                                Subject = sr.Subject,
                                ModifiedBy = sr.ModifiedBy,
                                CreatedBy = sr.CreatedBy,
                                EntityId = sr.EntityId,
                                Description = sr.Description,
                                UserName = user.FullName,
                                Comments = sr.Comments
                            };

                _logger.LogInformation("Applying sorting to the query.");
                query = sortBy switch
                {
                    ServiceRequestSortBy.Recent => query.OrderByDescending(x => x.CreatedOn),
                    ServiceRequestSortBy.Oldest => query.OrderBy(x => x.CreatedOn),
                    ServiceRequestSortBy.Red => query.OrderByDescending(x => x.CreatedOn),
                    ServiceRequestSortBy.Amber => query.OrderByDescending(x => x.CreatedOn),
                    ServiceRequestSortBy.Green => query.OrderByDescending(x => x.CreatedOn),
                    ServiceRequestSortBy.Subject => query.OrderBy(x => x.Subject),
                    ServiceRequestSortBy.Description => query.OrderBy(x => x.Description),
                    ServiceRequestSortBy.ExpectedDate => query.OrderByDescending(x => x.ExpectedDate),
                    _ => query.OrderByDescending(x => x.CreatedOn)
                };

                _logger.LogInformation("Executing query to fetch ServiceRequests.");
                var result = await query.ToListAsync();
                _logger.LogInformation("Successfully fetched {Count} ServiceRequests.", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching ServiceRequests with sortBy: {SortBy}", sortBy);
                return new List<Dto.ServiceRequest>();
            }
        }

        public async Task<List<Db.LevelMaster>> GetAllLevelAsync()
        {
            var levelMaster = new List<Db.LevelMaster>();
            try
            {
                levelMaster = await _dbContext.LevelMasters.Where(v => v.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving LevelMasters: {Message}", ex.Message);
            }
            return levelMaster;
        }

        public async Task<List<ServiceRequestDetails>> GetServiceRequestsByEntityAsync(long entityId)
        {
            try
            {
                var serviceRequests = await _dbContext.ServiceRequests
                    .Where(sr => sr.EntityId == entityId)
                    .ToListAsync();

                // Fetch related data from respective tables
                var entityIds = serviceRequests.Select(sr => sr.EntityId).Distinct().ToList();
                var orgIds = serviceRequests.Select(sr => sr.OrganizationId).Distinct().ToList();
                var userIds = serviceRequests.Select(sr => sr.ModifiedBy).Distinct().ToList();

                var entities = await _dbContext.Entity
                    .Where(e => entityIds.Contains(e.Id))
                    .ToDictionaryAsync(e => e.Id, e => e.EntityName);

                var organizations = await _dbContext.Organizations
                    .Where(o => orgIds.Contains(o.Id))
                    .ToDictionaryAsync(o => o.Id, o => o.OrganizationName);

                var users = await _dbContext.Users
                    .Where(u => userIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id, u => u.FullName);

                // Map ServiceRequest to ServiceRequestDetails
                var result = serviceRequests.Select(sr => new ServiceRequestDetails
                {
                    Id = sr.Id,
                    Subject = sr.Subject,
                    Description = sr.Description,
                    Status = sr.Status,
                    CreatedOn = sr.CreatedOn,
                    ExpectedDate = sr.ExpectedDate,
                    EntityName = entities.TryGetValue(sr.EntityId, out var entityName) ? entityName : null,
                    OrganizationName = organizations.TryGetValue(sr.OrganizationId, out var orgName) ? orgName : null,
                    UserName = (sr.ModifiedBy.HasValue && users.TryGetValue(sr.ModifiedBy.Value, out var userName)) ? userName : null,
                    Comments = sr.Comments,
                    StatusDisplay = Enum.IsDefined(typeof(LevelType), (int)sr.LevelMasterId)
    ? Enum.GetName(typeof(LevelType), (int)sr.LevelMasterId)
    : sr.LevelMasterId.ToString()

                }).ToList();

                return result;
            }
            catch (Exception)
            {
                return new List<ServiceRequestDetails>();
            }
        }
    }
}