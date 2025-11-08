using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ComplianceAPI.Repository
{
    public interface ICountryRepository
    {
        /// <summary>
        /// Get all country master data for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Country>> GetAllCountryMaster(int userId);

        Task<List<Country>> GetAllCountries();

        Task<Country> GetCountryByUID(Guid uid);

        Task<List<Country>> GetCountryByOrgId(int id);

        Task<States> GetStatesByCountry(int CountryCode);

        Task<Country> PostCountry(Country country);

        Task<bool> PostUpdateCountryApproval(AccessModel access, string ApprovalStatus);

        Task<States> PostState(States state);

        Task<string> SaveFileNamesAsync(int countryId, List<string> fileNames, int? createdBy);

        Task<bool> PostUpdateStateApproval(AccessModel access, string ApprovalStatus);

        /// <summary>
        /// Get states by country ID for a specific user
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<States>> GetStateById(int countryId, int userId);

        Task<List<CountryStateMappingModel>> GetCountryStatesMapping();

        Task<CountryStateMappingModel> PostCountryStateMapping(CountryStateMappingModel country);

        Task<CountryStateMappingModel> DeleteCountryStateMapping(CountryStateMappingModel country);

        Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID);

        Task<List<CountryStateApproval>> GetStateApprovaList(Guid UserUID);

        Task<List<CountryStateApproval>> GetAllCountryApprovaList();

        Task<List<CountryStateApproval>> GetAllStateApprovaList();

        Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID);

        Task<List<CountryStateApproval>> GetAllCountryStateMappingApprovaList();

        Task<bool> PostCountryStateApprovalMapping(AccessModel access, string ApprovalStatus);

        Task<List<CurrencyCodes>> GetAllCurrencyCodes();

        Task<StatusCountModel> GetServiceReqAndBillingDetails(long userId, long countryId);
        Task<StatusCountModel> GetServiceReqAndBillingDetailsByState(long userId, long stateId);
        Task<bool> PostUpdateCountryFileApproval(AccessModel access, Models.Enums.RefApprovalStatus approvalStatus);

        /// <summary>
        /// Post user country mapping data
        /// </summary>
        /// <param name="userCountryMappings"></param>
        /// <returns></returns>
        Task<bool> PostUserCountryMapping(List<UserCountryMappingModel> userCountryMappings);

        /// <summary>
        /// Post user state mapping data
        /// </summary>
        /// <param name="userStateMappings"></param>
        /// <returns></returns>
        Task<bool> PostUserStateMapping(List<UserStateMappingModel> userStateMappings);

        /// <summary>
        /// Get user state mapping data for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserStateMappingResponse>> GetUserStateMapping(int userId);

        Task<bool> AddCountryApprovalNotification(long createdBy, string countryName);

        Task<bool> AddStateApprovalNotification(int createdBy, string? stateName);

        Task<bool> AddCountryStateMappingApprovalNotification(int? createdBy, string? countryName, string? stateName);


        //Task<bool> PostApproveRejectState(AccessModel access);

        Task<bool> AddCountryApprovalSuccessNotification(long countryId, string approvalStatus);
        Task<bool> AddStateApprovalSuccessNotification(long stateId, string approvalStatus);
        Task<bool> AddCountryStateMappingApprovalSuccessNotification(long stateId, string approvalStatus);
        Task<string> GetNextStateCode();
        Task<string> GetNextCountryCode();

        Task<string> GetLastCountryReferenceCode();
        Task<string> GetLastStateReferenceCode();
    }

    public class CountryRepository : ICountryRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly ComplianceDbContext dbContext;
        private readonly ILogger<CountryRepository> _logger;
        private readonly IHelperRepository _helperRepository;

        public CountryRepository(IUnitOfWork unitOfWork, ComplianceDbContext dbContext, ILogger<CountryRepository> logger, IHelperRepository helperRepository)
        {
            _unitOfWork = unitOfWork;
            this.dbContext = dbContext;
            _logger = logger;
            _helperRepository = helperRepository;
        }

        ///<see cref="ICountryRepository.GetAllCountryMaster(int)"/>
        public async Task<List<Country>> GetAllCountryMaster(int userId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLCOUNTRYMASTER", new { UserId = userId }, commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<Country>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<List<Country>> GetAllCountries()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLCOUNTRY", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<Country>().ToList();
                mul.Dispose();
                return result;
            }
        }

        public async Task<Country> GetCountryByUID(Guid uid)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Country>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }
        }

        public async Task<States> GetStatesByCountry(int countryCode)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("USP_GETSTATEBYCOUNTRY", new { CountryCode = countryCode }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<States>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }
        }

        public async Task<List<Country>> GetCountryByOrgId(int id)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[organizations].[GETALLCOUNTRYBYORGID]", new { OrgId = id }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Country>().ToList();
                mul.Dispose();
                return Result;
            }
        }

        public async Task<Country> PostCountry(Country country)
        {
            try
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    var mul = await sqlContext.Connection.QueryFirstAsync<Country>("[product_owner].USP_POSTCOUNTRY", new
                    {
                        Id = country.Id,
                        CountryName = country.CountryName,
                        CountryCode = country.CountryCode,
                        CountryCodeNumber = country.CountryCodeNumber,
                        FinancialStartDate = country.FinancialStartDate,
                        FinancialEndDate = country.FinancialEndDate,
                        CurrencyId = country.CurrencyId,
                       // CurrencyCode = country.CurrencyCode,
                        CreatedBy = country.CreatedBy,
                        ModifiedBy = country.ModifiedBy,
                        UID = country.UID,
                        CountryReferenceCode = country.CountryReferenceCode,

                    }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();

                    return mul;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<string> SaveFileNamesAsync(int countryId, List<string> fileNames, int? createdBy)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    if (fileNames == null || !fileNames.Any())
                    {
                        var existingFiles = dbContext.countryFileNames
                                                     .Where(x => x.CountryId == countryId)
                                                     .ToList();

                        if (existingFiles.Any())
                        {
                            dbContext.countryFileNames.RemoveRange(existingFiles);
                            await dbContext.SaveChangesAsync();
                        }

                        sqlContext.Commit();
                        return "Successfully updated..";
                    }

                    // Get all existing files
                    var existingFilesForCountry = dbContext.countryFileNames
                                                          .Where(x => x.CountryId == countryId)
                                                          .ToList();

                    // Delete files not in the new list
                    foreach (var file in existingFilesForCountry.Where(f => !fileNames.Contains(f.FileName)))
                    {
                        dbContext.countryFileNames.Remove(file);
                    }

                    // Add new files that don’t already exist
                    foreach (var fileName in fileNames.Distinct())
                    {
                        var existing = existingFilesForCountry.FirstOrDefault(x => x.FileName == fileName);
                        if (existing == null)
                        {
                            var countryFile = new CountryFileNames
                            {
                                CountryId = countryId,
                                FileName = fileName,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.UtcNow,
                                Status = 0
                            };
                            dbContext.countryFileNames.Add(countryFile);
                        }
                        else
                        {
                            existing.ModifiedBy = createdBy;
                            existing.ModifiedOn = DateTime.UtcNow;
                            dbContext.countryFileNames.Update(existing);
                        }
                    }

                    // Save changes
                    await dbContext.SaveChangesAsync();
                    sqlContext.Commit();
                    return "Success";
                }
                catch (Exception)
                {
                    sqlContext.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> PostUpdateCountryApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, CountryId = access.CountryId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECOUNTRYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }

            return true;
        }

        public async Task<bool> PostUpdateCountryFileApproval(AccessModel access, Models.Enums.RefApprovalStatus approvalStatus)
        {
            var isAdmin = IsSuperAdmin(access.CreatedBy);
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    //get all files for the country
                    var files = dbContext.countryFileNames.Where(x => x.CountryId == access.CountryId && x.Status == 0).ToList();
                    if (files == null || files.Count == 0)
                    {
                        return false;
                    }
                    foreach (var file in files)
                    {
                        //check if approval already exists for this file
                        var approval = await dbContext.CountryFileNamesApproval.FirstOrDefaultAsync(x => x.CountryFileId == file.Id
                                    && x.ApprovalStatus == (int)Models.Enums.RefApprovalStatus.Pending);
                        if (approval == null)
                        {
                            dbContext.CountryFileNamesApproval.Add(new CountryFileNamesApproval
                            {
                                CountryFileId = file.Id.Value,
                                ManagerId = access.ManagerId.Value,
                                ApprovalStatus = isAdmin ? (int)Models.Enums.RefApprovalStatus.Approved : (int)Models.Enums.RefApprovalStatus.Pending,
                                CreatedBy = access.CreatedBy,
                                CreatedOn = DateTime.UtcNow,
                                UID = access.UID
                            });
                        }
                        else
                        {
                            approval.ManagerId = access.ManagerId.Value;
                            approval.ApprovalStatus = isAdmin ? (int)Models.Enums.RefApprovalStatus.Approved : (int)Models.Enums.RefApprovalStatus.Pending;
                            approval.ModifiedBy = access.CreatedBy;
                            approval.ModifiedOn = DateTime.UtcNow;
                            approval.UID = access.UID;
                            dbContext.CountryFileNamesApproval.Update(approval);
                        }

                        //update the file status based on approval
                        if (isAdmin || approvalStatus == Models.Enums.RefApprovalStatus.Approved)
                        {
                            var countryFile = await dbContext.countryFileNames.FirstOrDefaultAsync(x => x.Id == file.Id);
                            countryFile.Status = (int)Models.Enums.RefApprovalStatus.Approved;
                            countryFile.ModifiedBy = access.CreatedBy;
                            countryFile.ModifiedOn = DateTime.UtcNow;
                            dbContext.countryFileNames.Update(countryFile);
                        }

                        //if rejected, update the file status to rejected
                        if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected)
                        {
                            var countryFile = await dbContext.countryFileNames.FirstOrDefaultAsync(x => x.Id == file.Id);
                            countryFile.Status = (int)Models.Enums.RefApprovalStatus.Rejected;
                            countryFile.ModifiedBy = access.CreatedBy;
                            countryFile.ModifiedOn = DateTime.UtcNow;
                            dbContext.countryFileNames.Update(countryFile);
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    sqlContext.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<States> PostState(States state)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<States>(
    "[product_owner].USP_POSTSTATE", new
    {
        Id = state.Id,
        CountryId = state.CountryId,
        StateName = state.StateName,
        StateCode = state.StateCode,
        StateReferenceCode = state.StateReferenceCode, // ✅ already here
        CreatedBy = state.CreatedBy,
        ModifiedBy = state.ModifiedBy,
        UID = state.UID,
    },
    commandType: System.Data.CommandType.StoredProcedure,
    transaction: sqlContext.Transaction
).ConfigureAwait(false);

                sqlContext.Commit();

                return mul;
            }
        }

        public async Task<bool> PostUpdateStateApproval(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, StateId = access.StateId, StateApprovalReferenceCode = access.ReferenceCode };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATESTATEAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }
            return true;
        }

        /// <see cref="ICountryRepository.GetStateById(int, int)"/>
        public async Task<List<States>> GetStateById(int countryId, int userId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETSTATEBYID", new { CountryId = countryId, UserId = userId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<States>().ToList();
                mul.Dispose();
                return Result;
            }
        }

        public async Task<List<CountryStateMappingModel>> GetCountryStatesMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCountryStateMapping", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateMappingModel>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<CountryStateMappingModel> PostCountryStateMapping(CountryStateMappingModel country)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<CountryStateMappingModel>("[product_owner].USP_POSTCountryStateMapping", new
                {
                    Id = country.Id,
                    CountryId = country.CountryId,
                    StateId = country.StateId,
                    CreatedBy = country.CreatedBy,
                    UID = country.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }

        public async Task<bool> PostCountryStateApprovalMapping(AccessModel access, string ApprovalStatus)
        {
            try
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATECountryStateMappingAPPROVAL", new
                    {
                        CountryStateMappingId = access.CountryStateMappingId,
                        ManagerId = access.ManagerId,
                        CreatedBy = access.CreatedBy,
                        UID = access.UID,
                        ApprovalStatus = ApprovalStatus,
                    }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    sqlContext.Commit();
                    
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<CountryStateApproval>> GetCountryApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<CountryStateApproval>> GetAllCountryApprovaList()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETALLCOUNTRYAPPROVALLIST", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<CountryStateApproval>> GetAllStateApprovaList()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETALLSTATEAPPROVALLIST", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<CountryStateApproval>> GetStateApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETSTATEAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<CountryStateApproval>> GetCountryStateMappingApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCountryStateMappingAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<CountryStateApproval>> GetAllCountryStateMappingApprovaList()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETAllCountryStateMappingAPPROVALLIST", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<CountryStateApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<CountryStateMappingModel> DeleteCountryStateMapping(CountryStateMappingModel country)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<CountryStateMappingModel>("[product_owner].USP_DELETECountryStateMapping", new
                {
                    Id = country.Id,
                    ModifiedBy = country.ModifiedBy,
                    UID = country.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }

        public async Task<List<CurrencyCodes>> GetAllCurrencyCodes()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                return await dbContext.CurrencyCodes.ToListAsync();
            }
        }

        public bool IsSuperAdmin(long? userId)
        {
            var isSuperAdmin = (from user in dbContext.Users
                                join userRoleMapping in dbContext.UserRoleMapping on user.Id equals userRoleMapping.UserId
                                join refRole in dbContext.RefRoles on userRoleMapping.RoleId equals Convert.ToInt32(refRole.Id)
                                where user.Id == userId && (refRole.RoleName == "SuperAdmin" || refRole.RoleName == "ITSupportAdmin")
                                select user).Any();

            return isSuperAdmin;
        }

        ///<see cref="ICountryRepository.PostUserCountryMapping(List{UserCountryMappingModel})"/>
        public async Task<bool> PostUserCountryMapping(List<UserCountryMappingModel> userCountryMappings)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    foreach (var userCountryMapping in userCountryMappings)
                    {
                        var inputdata = new { UserId = userCountryMapping.UserId, ContryId = userCountryMapping.CountryId, HasAccess = userCountryMapping.HasAccess };
                        var obj = await sqlContext.Connection.QueryAsync<object>("[dbo].[USP_POSTUSERCOUNTRY]", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    }
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }
            return true;
        }

        ///<see cref="ICountryRepository.PostUserStateMapping(List{UserStateMappingModel})"/>
        public async Task<bool> PostUserStateMapping(List<UserStateMappingModel> userStateMappings)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    foreach (var userStateMapping in userStateMappings)
                    {
                        var inputdata = new { UserId = userStateMapping.UserId, StateId = userStateMapping.StateId, HasAccess = userStateMapping.HasAccess };
                        var obj = await sqlContext.Connection.QueryAsync<object>("[dbo].[USP_POSTUSERSTATE]", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                    }
                    sqlContext.Commit();
                }
                catch (Exception ex)
                {
                    sqlContext.Rollback();
                    throw ex;
                }
            }
            return true;
        }

        ///<see cref="ICountryRepository.GetUserStateMapping(int)"/>
        public async Task<List<UserStateMappingResponse>> GetUserStateMapping(int userId)
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var query = @"SELECT S.Id AS StateId, USM.UserId, S.StateName, USM.HasAccess
	                            FROM UserStateMapping USM INNER JOIN product_owner.State S
	                            ON S.Id = USM.StateId WHERE USM.UserId=@UserId"
                ;

                var result = await connection.QueryAsync<UserStateMappingResponse>(query, new { UserId = userId });
                return result.ToList();
            }
        }

        public async Task<StatusCountModel> GetServiceReqAndBillingDetails(long userId, long countryId)
        {
            // Get all entities for the user in the specified Country
            var userEntities = await dbContext.Entity
                .Where(e => e.OrganizationId != null && e.CountryId == countryId)
                .Select(e => e.Id)
                .ToListAsync();

            int serviceRequestAmber = 0, serviceRequestRed = 0, serviceRequestGreen = 0, serviceRequestTotal = 0;

            // Gather all service requests for these entities
            var serviceRequests = await dbContext.ServiceRequests
                .Where(sr => userEntities.Contains(sr.EntityId))
                .ToListAsync();

            // Fetch related data for mapping
            var entityIds = serviceRequests.Select(sr => sr.EntityId).Distinct().ToList();
            var orgIds = serviceRequests.Select(sr => sr.OrganizationId).Distinct().ToList();
            var userIds = serviceRequests.Select(sr => sr.ModifiedBy).Distinct().ToList();

            var entities = await dbContext.Entity
                .Where(e => entityIds.Contains(e.Id))
                .ToDictionaryAsync(e => e.Id, e => e.EntityName);

            var organizations = await dbContext.Organizations
                .Where(o => orgIds.Contains(o.Id))
                .ToDictionaryAsync(o => o.Id, o => o.OrganizationName);

            var users = await dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            // Map ServiceRequest to ServiceRequestDetails and count by StatusDisplay
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

            foreach (var sr in result)
            {
                if (string.Equals(sr.StatusDisplay, "Red", StringComparison.OrdinalIgnoreCase))
                    serviceRequestRed++;
                else if (string.Equals(sr.StatusDisplay, "Amber", StringComparison.OrdinalIgnoreCase))
                    serviceRequestAmber++;
                else if (string.Equals(sr.StatusDisplay, "Green", StringComparison.OrdinalIgnoreCase))
                    serviceRequestGreen++;

                serviceRequestTotal++;
            }

            // Prepare bill status dictionary once
            var billStatusDict = await dbContext.BillStatus.ToDictionaryAsync(bs => bs.Id, bs => bs.BillStatusName);

            int billingDetailsAmber = 0, billingDetailsRed = 0, billingDetailsGreen = 0, billingDetailsTotal = 0;
            var now = DateTime.UtcNow.Date;
            var nowPlus7 = now.AddDays(7);

            // Get all billing details for all entities in one query
            var allBillingDetails = await dbContext.BillingDetails
                .Where(bd => bd.CountryId == countryId && userEntities.Contains(bd.EntityId ?? 0))
                .ToListAsync();

            foreach (var b in allBillingDetails)
            {
                var billStatusName = b.BillStatus.HasValue && billStatusDict.TryGetValue(b.BillStatus.Value, out var statusName) ? statusName : null;

                LevelType color = LevelType.Green;
                if (string.Equals(billStatusName, "Received", StringComparison.OrdinalIgnoreCase))
                {
                    color = LevelType.Green;
                }
                else if (b.DueDate.HasValue)
                {
                    var dueDate = b.DueDate.Value.Date;
                    if (dueDate < now)
                        color = LevelType.Red;
                    else if (dueDate <= nowPlus7)
                        color = LevelType.Amber;
                    else
                        color = LevelType.Green;
                }

                // Count by color
                if (color == LevelType.Red)
                    billingDetailsRed++;
                else if (color == LevelType.Amber)
                    billingDetailsAmber++;
                else if (color == LevelType.Green)
                    billingDetailsGreen++;

                billingDetailsTotal++;
            }

            // Find entities with no service requests and add them as Amber
            var entitiesWithServiceRequests = serviceRequests.Select(sr => sr.EntityId).Distinct().ToHashSet();
            var entitiesWithoutServiceRequests = userEntities.Where(eid => !entitiesWithServiceRequests.Contains(eid)).ToList();

            serviceRequestAmber += entitiesWithoutServiceRequests.Count;
            serviceRequestTotal += entitiesWithoutServiceRequests.Count;

            // Find entities with no billing details and add them as Amber
            var entitiesWithBillingDetails = allBillingDetails.Where(bd => bd.EntityId.HasValue).Select(bd => bd.EntityId.Value).Distinct().ToHashSet();
            var entitiesWithoutBillingDetails = userEntities.Where(eid => !entitiesWithBillingDetails.Contains(eid)).ToList();

            billingDetailsAmber += entitiesWithoutBillingDetails.Count;
            billingDetailsTotal += entitiesWithoutBillingDetails.Count;

            return new StatusCountModel
            {
                CountryId = (int)countryId,
                ServiceRequestAmber = serviceRequestAmber,
                ServiceRequestRed = serviceRequestRed,
                ServiceRequestGreen = serviceRequestGreen,
                BillingDetailsAmber = billingDetailsAmber,
                BillingDetailsRed = billingDetailsRed,
                BillingDetailsGreen = billingDetailsGreen,
                ServiceRequestTotal = serviceRequestTotal,
                BillingDetailsTotal = billingDetailsTotal
            };
        }
        public async Task<StatusCountModel> GetServiceReqAndBillingDetailsByState(long userId, long stateId)
        {
            // Get all entities for the user in the specified State
            var userEntities = await dbContext.Entity
                .Where(e => e.OrganizationId != null && e.StateId == stateId)
                .Select(e => e.Id)
                .ToListAsync();

            int serviceRequestAmber = 0, serviceRequestRed = 0, serviceRequestGreen = 0, serviceRequestTotal = 0;

            // Gather all service requests for these entities
            var serviceRequests = await dbContext.ServiceRequests
                .Where(sr => userEntities.Contains(sr.EntityId))
                .ToListAsync();

            // Fetch related data for mapping
            var entityIds = serviceRequests.Select(sr => sr.EntityId).Distinct().ToList();
            var orgIds = serviceRequests.Select(sr => sr.OrganizationId).Distinct().ToList();
            var userIds = serviceRequests.Select(sr => sr.ModifiedBy).Distinct().ToList();

            var entities = await dbContext.Entity
                .Where(e => entityIds.Contains(e.Id))
                .ToDictionaryAsync(e => e.Id, e => e.EntityName);

            var organizations = await dbContext.Organizations
                .Where(o => orgIds.Contains(o.Id))
                .ToDictionaryAsync(o => o.Id, o => o.OrganizationName);

            var users = await dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            // Map ServiceRequest to ServiceRequestDetails and count by StatusDisplay
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

            foreach (var sr in result)
            {
                if (string.Equals(sr.StatusDisplay, "Red", StringComparison.OrdinalIgnoreCase))
                    serviceRequestRed++;
                else if (string.Equals(sr.StatusDisplay, "Amber", StringComparison.OrdinalIgnoreCase))
                    serviceRequestAmber++;
                else if (string.Equals(sr.StatusDisplay, "Green", StringComparison.OrdinalIgnoreCase))
                    serviceRequestGreen++;

                serviceRequestTotal++;
            }

            // Prepare bill status dictionary once
            var billStatusDict = await dbContext.BillStatus.ToDictionaryAsync(bs => bs.Id, bs => bs.BillStatusName);

            int billingDetailsAmber = 0, billingDetailsRed = 0, billingDetailsGreen = 0, billingDetailsTotal = 0;
            var now = DateTime.UtcNow.Date;
            var nowPlus7 = now.AddDays(7);

            // Get all billing details for all entities in one query
            var allBillingDetails = await dbContext.BillingDetails
                .Where(bd => bd.StateId == stateId && userEntities.Contains(bd.EntityId ?? 0))
                .ToListAsync();

            foreach (var b in allBillingDetails)
            {
                var billStatusName = b.BillStatus.HasValue && billStatusDict.TryGetValue(b.BillStatus.Value, out var statusName) ? statusName : null;

                LevelType color = LevelType.Green;
                if (string.Equals(billStatusName, "Received", StringComparison.OrdinalIgnoreCase))
                {
                    color = LevelType.Green;
                }
                else if (b.DueDate.HasValue)
                {
                    var dueDate = b.DueDate.Value.Date;
                    if (dueDate < now)
                        color = LevelType.Red;
                    else if (dueDate <= nowPlus7)
                        color = LevelType.Amber;
                    else
                        color = LevelType.Green;
                }

                // Count by color
                if (color == LevelType.Red)
                    billingDetailsRed++;
                else if (color == LevelType.Amber)
                    billingDetailsAmber++;
                else if (color == LevelType.Green)
                    billingDetailsGreen++;

                billingDetailsTotal++;
            }

            // Find entities with no service requests and add them as Amber
            var entitiesWithServiceRequests = serviceRequests.Select(sr => sr.EntityId).Distinct().ToHashSet();
            var entitiesWithoutServiceRequests = userEntities.Where(eid => !entitiesWithServiceRequests.Contains(eid)).ToList();

            serviceRequestAmber += entitiesWithoutServiceRequests.Count;
            serviceRequestTotal += entitiesWithoutServiceRequests.Count;

            // Find entities with no billing details and add them as Amber
            var entitiesWithBillingDetails = allBillingDetails.Where(bd => bd.EntityId.HasValue).Select(bd => bd.EntityId.Value).Distinct().ToHashSet();
            var entitiesWithoutBillingDetails = userEntities.Where(eid => !entitiesWithBillingDetails.Contains(eid)).ToList();

            billingDetailsAmber += entitiesWithoutBillingDetails.Count;
            billingDetailsTotal += entitiesWithoutBillingDetails.Count;

            return new StatusCountModel
            {
                StateId = (int)stateId,
                ServiceRequestAmber = serviceRequestAmber,
                ServiceRequestRed = serviceRequestRed,
                ServiceRequestGreen = serviceRequestGreen,
                BillingDetailsAmber = billingDetailsAmber,
                BillingDetailsRed = billingDetailsRed,
                BillingDetailsGreen = billingDetailsGreen,
                ServiceRequestTotal = serviceRequestTotal,
                BillingDetailsTotal = billingDetailsTotal
            };
        }

        public async Task<bool> AddCountryApprovalNotification(long createdBy, string countryName)
        {
            try
            {
                _logger.LogInformation("AddCountryApprovalNotification started for CreatedBy: {CreatedBy}", createdBy);

                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);

                var notification = new Notification
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    NotificationTitle = createdByUserDetails.UserRoleId == 1 ?
                    string.Format(ApiConstants.NewCountryBySuperAdminNotificationTitleTemplate, countryName)
                    :
                    string.Format(ApiConstants.NewCountryNotificationTitleTemplate, createdByUserDetails.UserName, countryName),

                    NotificationMessage = createdByUserDetails.UserRoleId == 1 ?
                    string.Format(ApiConstants.NewCountryBySuperAdminNotificationMessageTemplate, countryName)
                    :
                    string.Format(ApiConstants.NewCountryNotificationMessageTemplate, createdByUserDetails.UserName, countryName),

                    SenderUserId = createdByUserDetails.UserId,
                    SenderUserName = createdByUserDetails?.UserName,
                    RecipientUserId = createdByUserDetails.ManagerId!,
                    RecipientUserName = createdByUserDetails?.ManagerName,
                    CreatedDate = DateTime.UtcNow,
                    ModuleType = Models.Enums.ModuleType.Country,

                    Status = createdByUserDetails.UserRoleId == 1 ?
                    Models.Enums.RefApprovalStatus.Approved
                    :
                    Models.Enums.RefApprovalStatus.Pending,
                    MarkAsRead = false
                };
                await dbContext.AddAsync(notification);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Notification created successfully for CreatedBy: {CreatedBy}", createdBy);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddCountryApprovalNotification for CreatedBy: {CreatedBy}", createdBy);
                return false;
            }
        }

        public async Task<bool> AddStateApprovalNotification(int createdBy, string? stateName)
        {
            try
            {
                var user = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (user != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        NotificationTitle = user.UserRoleId == 1 ?
                        string.Format(ApiConstants.NewStateBySuperAdminNotificationTitleTemplate, stateName)
                        :
                        string.Format(ApiConstants.NewStateNotificationTitleTemplate, user.UserName, stateName),

                        NotificationMessage = user.UserRoleId == 1 ?
                        string.Format(ApiConstants.NewStateBySuperAdminNotificationMessageTemplate, user.UserName, stateName)
                        :
                        string.Format(ApiConstants.NewStateNotificationMessageTemplate, stateName),

                        SenderUserId = user.UserId,
                        SenderUserName = user.UserName,
                        RecipientUserId = user.ManagerId,
                        RecipientUserName = user.ManagerName,
                        ModuleType = ModuleType.State,
                        CreatedDate = DateTime.UtcNow,
                        Status = user.UserRoleId == 1 ?
                        Models.Enums.RefApprovalStatus.Approved
                        :
                        Models.Enums.RefApprovalStatus.Pending,
                        MarkAsRead = false
                    };
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> AddCountryStateMappingApprovalNotification(int? createdBy, string? countryName, string? stateName)
        {
            try
            {
                var createdUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy.Value);
                if (createdUserDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdUserDetails.UserId,
                        SenderUserName = createdUserDetails.UserName,
                        RecipientUserId = createdUserDetails.ManagerId,
                        RecipientUserName = createdUserDetails.ManagerName,
                        ModuleType = Models.Enums.ModuleType.StateMapping,
                        CreatedDate = DateTime.UtcNow,
                        MarkAsRead=false
                    };
                    if (createdUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryStateMappingBySuperAdminNotificationTitleTemplate, stateName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewCountryStateMappingBySuperAdminNotificationMessageTemplate, stateName);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryStateMappingNotificationTitleTemplate, createdUserDetails.UserName, stateName, countryName);
                        notification.NotificationMessage= string.Format(ApiConstants.NewCountryStateMappingNotificationMessageTemplate, createdUserDetails.UserName, stateName, countryName);
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
                    }
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddCountryApprovalSuccessNotification(long countryId, string approvalStatus)
        {
            try
            {
                var countryDetails = await dbContext.Country.FindAsync((int)countryId);
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(countryDetails.CreatedBy.Value);
                if (createdByUserDetails != null && countryDetails!=null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.ManagerId,
                        SenderUserName = createdByUserDetails.ManagerName,
                        RecipientUserId = createdByUserDetails.UserId,
                        RecipientUserName = createdByUserDetails.ManagerName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.Country
                    };
                    if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryApprovedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewCountryApprovedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryRejectNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewCountryRejectNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Forward;
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryForwardNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewCountryForwardNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddStateApprovalSuccessNotification(long stateId, string approvalStatus)
        {
            try
            {
                var stateDetails = await dbContext.States.FindAsync(stateId);
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(stateDetails.CreatedBy.Value);
                if (createdByUserDetails != null && stateDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.ManagerId,
                        SenderUserName = createdByUserDetails.ManagerName,
                        RecipientUserId = createdByUserDetails.UserId,
                        RecipientUserName = createdByUserDetails.ManagerName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.State
                    };
                    if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.NewStateApprovedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewStateApprovedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = string.Format(ApiConstants.NewStateRejectNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewStateRejectNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Forward;
                        notification.NotificationTitle = string.Format(ApiConstants.NewStateForwardNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewStateForwardNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }    
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddCountryStateMappingApprovalSuccessNotification(long stateId, string approvalStatus)
        {
            try
            {
                var stateDetails = await dbContext.States.FindAsync(stateId);
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(stateDetails.CreatedBy.Value);
                if (createdByUserDetails != null && stateDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.ManagerId,
                        SenderUserName = createdByUserDetails.ManagerName,
                        RecipientUserId = createdByUserDetails.UserId,
                        RecipientUserName = createdByUserDetails.ManagerName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.State
                    };
                    if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryStateMappingApprovedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewCountryStateMappingApprovedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else if (approvalStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryStateMappingRejectNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewCountryStateMappingRejectNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Forward;
                        notification.NotificationTitle = string.Format(ApiConstants.NewCountryStateMappingForwardNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewCountryStateMappingForwardNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    await dbContext.AddAsync(notification);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> GetLastCountryReferenceCode()
        {
            // Get the max Id from Country table
            var maxId = await dbContext.Country.MaxAsync(e => (long?)e.Id) ?? 0;
            // Increment by 1
            var nextId = maxId + 1;
            // Format as 'C' + right 3 digits, padded with zeros
            var lastCode = $"C{nextId.ToString().PadLeft(3, '0')}";
            return lastCode;
        }

        public async Task<string> GetLastStateReferenceCode()
        {
            // Get the max Id from Country table
            var maxId = await dbContext.States.MaxAsync(e => (long?)e.Id) ?? 0;
            // Increment by 1
            var nextId = maxId + 1;
            // Format as 'S' + right 3 digits, padded with zeros
            var lastCode = $"{ReferencePrefixes.State}{nextId.ToString().PadLeft(3, '0')}";
            return lastCode;
        }


        public async Task<string> GetNextCountryCode()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[Country]");
                int nextNumber = count + 1;
                return $"{ReferencePrefixes.Country}{nextNumber.ToString("D3")}";
            }
        }


        public async Task<string> GetNextStateCode()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[State]");
                int nextNumber = count + 1;
                return $"{ReferencePrefixes.State}{nextNumber.ToString("D3")}";
            }
        }
    }
}