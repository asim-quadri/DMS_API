using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using Dto= ComplianceAPI.Models;
using Db = ComplianceAPI.Models.DataModels;
using Dapper;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models;
using ComplianceAPI.Models.Enums;

namespace ComplianceAPI.Repository
{
    public interface IIndustryRepository
    {
        Task<List<Dto.MajorIndustry>> GetAllMajorIndustry();
        Task<Dto.MajorIndustry> PostMajorIndustry(Dto.MajorIndustry majorIndustry);
        Task<bool> PostUpdateIndustryApproval(Dto.AccessModel access, string ApprovalStatus);
        Task<bool> PostUpdateMajorIndustryApproval(Dto.AccessModel access, string ApprovalStatus);
        Task<bool> PostUpdateMinorIndustryApproval(Dto.AccessModel access, string ApprovalStatus);
        Task<Dto.MinorIndustry> PostMinorIndustry(Dto.MinorIndustry minorIndustry);
        Task<List<Dto.MajorIndustry>> GetMajorIndustryById(int countryId);
        Task<List<Dto.MinorIndustry>> GetMinorIndustryById(int majorIndustryId);
        Task<Dto.MajorIndustry> GetMajorIndustryByUID(Guid uid);
        Task<List<Dto.CountryMajorMapping>> GetCountryMajorMapping();
        Task<List<Dto.IndustryrMapping>> GetIndustryMapping();
        Task<List<Dto.IndustryrMapping>> GetIndustryMappingByMajor(long? majorInudustryId, long? countryId);
        Task<List<Dto.IndustryrMapping>> GetIndustryMappingByCountry(long? countryId);
        Task<Dto.IndustryrMapping> PostIndustryMapping(Dto.IndustryrMapping industryMapping);
        Task<List<Dto.MajorMinorMapping>> GetMajorMinorMapping();
        Task<Dto.MajorMinorMapping> PostMajorMinorMapping(Dto.MajorMinorMapping majorMinorMapping);
        Task<List<Dto.MajorMinorApproval>> GetMajorApprovaList(Guid UserUID);
        Task<List<Dto.MajorMinorApproval>> GetMinorApprovaList(Guid UserUID);
        Task<List<Dto.IndustryApproval>> GetIndustryMappingApprovaList(Guid UserUID);
        Task<List<Dto.CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID);
        Task<List<Dto.MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID);
        Task<bool> PostIndustryApprovalMapping(Dto.AccessModel access, string ApprovalStatus);
        Task<bool> PostMajorMinorApprovalMapping(Dto.AccessModel access, string ApprovalStatus);
        //Task<States> GetStatesByCountry(int CountryCode);
        Task<bool> AddMajorIndustryApprovalNotification(int createdBy, string majorIndustryName);
        Task<bool> AddMinorIndustryApprovalNotification(int createdBy, string minorIndustryName);

        Task<bool> AddCountryMajorMappingApprovalNotification(int createdBy, long minorIndustryId);
        Task<bool> AddMajorIndustryApprovalSuccessNotification(long majorIndustryId, string approveStatus);
        Task<bool> AddMinorIndustryMappingApprovalSuccessNotification(long minorIndustryId, string approveStatus);
        Task<bool> AddMinorIndustryApprovalSuccessNotification(long minorIndustryId, string approveStatus);
        Task<string> GetNextMinorIndustryCode();
        Task<string> GetNextMajorIndustryCode();

    }
    public class IndustryRepository : IIndustryRepository
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IHelperRepository _helperRepository;
        private readonly Db.ComplianceDbContext _dbContext;
        public IndustryRepository(IUnitOfWork unitOfWork, IHelperRepository helperRepository, Db.ComplianceDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _helperRepository = helperRepository;
            _dbContext = dbContext;
        }
        public async Task<List<Dto.MajorIndustry>> GetAllMajorIndustry()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var mul = await connection.QueryMultipleAsync("[product_owner].USP_GETALLMAJORINDUSTRY", commandType: System.Data.CommandType.StoredProcedure);
                var result = mul.Read<Dto.MajorIndustry>().ToList();
                mul.Dispose();
                return result;
            }
        }
        public async Task<Dto.MajorIndustry> PostMajorIndustry(Dto.MajorIndustry majorIndustry)
        {
            try
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    var mul = await sqlContext.Connection.QueryFirstAsync<Dto.MajorIndustry>("[product_owner].USP_POSTMAJORINDUSTRY", new
                    {
                        Id = majorIndustry.Id,
                        MajorIndustryName = majorIndustry.MajorIndustryName,
                        MajorIndustryCode = majorIndustry.MajorIndustryCode,
                        CreatedBy = majorIndustry.CreatedBy,
                        ModifiedBy = majorIndustry.ModifiedBy,
                        UID = majorIndustry.UID,
                        MajorIndustryReferenceCode = majorIndustry.MajorIndustryReferenceCode
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
        public async Task<Dto.MinorIndustry> PostMinorIndustry(Dto.MinorIndustry minorIndustry)
        {
            try
            {
                using (var sqlContext = _unitOfWork.ContextFactory())
                {
                    var mul = await sqlContext.Connection.QueryFirstAsync<Dto.MinorIndustry>(
                        "[product_owner].USP_POSTMINORINDUSTRY",
                        new
                        {
                            Id = minorIndustry.Id,
                            MajorIndustryId = minorIndustry.MajorIndustryId,
                            MinorIndustryName = minorIndustry.MinorIndustryName,
                            MinorIndustryCode = minorIndustry.MinorIndustryCode,
                            MinorIndustryReferenceCode = minorIndustry.MinorIndustryReferenceCode, // <-- Make sure this is string
                            CreatedBy = minorIndustry.CreatedBy,
                            ModifiedBy = minorIndustry.ModifiedBy,
                            UID = minorIndustry.UID
                        },
                        commandType: System.Data.CommandType.StoredProcedure,
                        transaction: sqlContext.Transaction
                    ).ConfigureAwait(false);

                    sqlContext.Commit();

                    return mul;
                }
            }
            catch(Exception ex)
            {
                return null;
            }        


        }
        public async Task<List<Dto.MajorIndustry>> GetMajorIndustryById(int countryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORINDUSTRYBYID", new { CountryId = countryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.MajorIndustry>().ToList();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<List<Dto.MinorIndustry>> GetMinorIndustryById(int majorIndustryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMINORINDUSTRYBYID", new { MajorIndustryId = majorIndustryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.MinorIndustry>().ToList();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<Dto.MajorIndustry> GetMajorIndustryByUID(Guid uid)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORINDUSTRYBYUID", new { UID = uid }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.MajorIndustry>().FirstOrDefault();
                mul.Dispose();
                return Result;
            }
        }
        public async Task<bool> PostUpdateIndustryApproval(Dto.AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, MajorIndustryId = access.MajorIndustryId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEMAJORINDUSTRYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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
        public async Task<bool> PostUpdateMajorIndustryApproval(Dto.AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, MajorIndustryId = access.MajorIndustryId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEMAJORINDUSTRYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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
        public async Task<bool> PostUpdateMinorIndustryApproval(Dto.AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                try
                {
                    var inputdata = new { ManagerId = access.ManagerId, CreatedBy = access.CreatedBy, UID = access.UID, ApprovalStatus = ApprovalStatus, MinorIndustryId = access.MinorIndustryId };
                    var obj = await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEMINORINDUSTRYAPPROVAL", inputdata, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
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
        public async Task<List<Dto.CountryMajorMapping>> GetCountryMajorMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYMAJORINDUSTRYMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.CountryMajorMapping>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<List<Dto.IndustryrMapping>> GetIndustryMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETINDUSTRYMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.IndustryrMapping>();
                mul.Dispose();
                return Result.ToList();
            }

        }
        public async Task<List<Dto.IndustryrMapping>> GetIndustryMappingByMajor(long? majorInudustryId, long? countryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETINDUSTRYMAPPINGBYMAJOR", new { MajorIndustryId = majorInudustryId, CountryId = countryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.IndustryrMapping>();
                mul.Dispose();
                return Result.DistinctBy(x => x.MinorIndustryId).ToList();
            }

        }
        public async Task<List<Dto.IndustryrMapping>> GetIndustryMappingByCountry(long? countryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETINDUSTRYMAPPINGBYCOUNTRY", new { CountryId = countryId}, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.IndustryrMapping>();
                mul.Dispose();
                return Result.DistinctBy(x => x.MajorIndustryId).ToList();
            }

        }
        public async Task<Dto.IndustryrMapping> PostIndustryMapping(Dto.IndustryrMapping industryMapping)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<Dto.IndustryrMapping>("[product_owner].USP_POSTINDUSTRYMAPPING", new
                {
                    Id = industryMapping.Id,
                    CountryId = industryMapping.CountryId,
                    MajorIndustryId = industryMapping.MajorIndustryId,
                    MinorIndustryId = industryMapping.MinorIndustryId,
                    CreatedBy = industryMapping.CreatedBy,
                    UID = industryMapping.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }
        public async Task<List<Dto.IndustryApproval>> GetIndustryMappingApprovaList(Guid UserUID)

        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETINDUSTRYMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.IndustryApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<bool> PostIndustryApprovalMapping(Dto.AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEINDUSTRYMAPPINGAPPROVAL", new
                {
                    IndustryMappingId = access.IndustryMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return true;
            }
        }
        public async Task<bool> PostMajorMinorApprovalMapping(Dto.AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATEMAJORMINORMAPPINGAPPROVAL", new
                {
                    MajorMinorIndustryMappingId = access.MajorMinorIndustryMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                return true;
            }
        }
        public async Task<List<Dto.MajorMinorMapping>> GetMajorMinorMapping()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORMINORINDUSTRYMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.MajorMinorMapping>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<Dto.MajorMinorMapping> PostMajorMinorMapping(Dto.MajorMinorMapping majorMinorMapping)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<Dto.MajorMinorMapping>("[product_owner].USP_POSTMAJORMINORINDUSTRYMAPPING", new
                {
                    Id = majorMinorMapping.Id,
                    MajorIndustryId = majorMinorMapping.MajorIndustryId,
                    MinorIndustryId = majorMinorMapping.MinorIndustryId,
                    CreatedBy = majorMinorMapping.CreatedBy,
                    UID = majorMinorMapping.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();

                return mul;
            }
        }
        public async Task<List<Dto.MajorMinorApproval>> GetMajorApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORINDUSTRYAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.MajorMinorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<List<Dto.MajorMinorApproval>> GetMinorApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMINORINDUSTRYAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.MajorMinorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<List<Dto.CountryMajorApproval>> GetCountryMajorMappingApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETCOUNTRYMAJORINDUSTRYMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.CountryMajorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<List<Dto.MajorMinorApproval>> GetMajorMinorMappingApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETMAJORMINORINDUSTRYMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<Dto.MajorMinorApproval>();
                mul.Dispose();
                return Result.ToList();
            }
        }

        public async Task<bool> AddMajorIndustryApprovalNotification(int createdBy, string majorIndustryName)
        {
            try
            {
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (createdByUserDetails != null)
                {
                    var notification = new Db.Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        
                        NotificationTitle= createdByUserDetails.UserRoleId==1?
                        string.Format(ApiConstants.NewMajorIndustryBySuperAdminrNotificationTitleTemplate, majorIndustryName)
                        :
                        string.Format(ApiConstants.NewMajorIndustryNotificationTitleTemplate, createdByUserDetails.UserName, majorIndustryName),

                        NotificationMessage=createdByUserDetails.UserRoleId==1?
                        string.Format(ApiConstants.NewMajorIndustryBySuperAdminNotificationMessageTemplate, majorIndustryName)
                        :
                        string.Format(ApiConstants.NewMajorIndustryNotificationMessageTemplate, createdByUserDetails.UserName, majorIndustryName),

                        SenderUserId=createdByUserDetails.UserId,
                        SenderUserName=createdByUserDetails.UserName,
                        RecipientUserId=createdByUserDetails.ManagerId,
                        RecipientUserName=createdByUserDetails.ManagerName,
                        ModuleType=Models.Enums.ModuleType.MajorIndustry,
                        CreatedDate=DateTime.UtcNow,
                        ReadDate=null,
                       
                        Status=createdByUserDetails.UserRoleId==1?
                        Models.Enums.RefApprovalStatus.Pending
                        :
                        Models.Enums.RefApprovalStatus.Approved,

                        MarkAsRead=false
                    };
                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddMinorIndustryApprovalNotification(int createdBy, string minorIndustryName)
        {
            try
            {
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (createdByUserDetails != null)
                {
                    var notification = new Db.Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),

                        NotificationTitle = createdByUserDetails.UserRoleId == 1 ?
                        string.Format(ApiConstants.NewMinorIndustryBySuperAdminrNotificationTitleTemplate, minorIndustryName)
                        :
                        string.Format(ApiConstants.NewMinorIndustryNotificationTitleTemplate, createdByUserDetails.UserName, minorIndustryName),

                        NotificationMessage = createdByUserDetails.UserRoleId == 1 ?
                        string.Format(ApiConstants.NewMinorIndustryBySuperAdminNotificationMessageTemplate, minorIndustryName)
                        :
                        string.Format(ApiConstants.NewMinorIndustryNotificationMessageTemplate, createdByUserDetails.UserName, minorIndustryName),

                        SenderUserId = createdByUserDetails.UserId,
                        SenderUserName = createdByUserDetails.UserName,
                        RecipientUserId = createdByUserDetails.ManagerId,
                        RecipientUserName = createdByUserDetails.ManagerName,
                        ModuleType = Models.Enums.ModuleType.MajorIndustry,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,

                        Status = createdByUserDetails.UserRoleId == 1 ?
                        Models.Enums.RefApprovalStatus.Pending
                        :
                        Models.Enums.RefApprovalStatus.Approved,

                        MarkAsRead = false
                    };
                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddCountryMajorMappingApprovalNotification(int createdBy, long minorIndustryId)
        {
            try
            {
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(createdBy);
                var minorIndustry = await _dbContext.MinorIndustries.FindAsync(minorIndustryId);
                if (createdByUserDetails != null && minorIndustry!=null)
                {
                    var notification = new Notification
                    {
                        NotificationId=Guid.NewGuid().ToString(),
                        SenderUserId=createdByUserDetails.UserId,
                        SenderUserName=createdByUserDetails.UserName,
                        RecipientUserId=createdByUserDetails.ManagerId,
                        RecipientUserName=createdByUserDetails.ManagerName,
                        CreatedDate=DateTime.UtcNow,
                        ReadDate=null,
                        MarkAsRead=false,
                        ModuleType=Models.Enums.ModuleType.IndustryMapping
                    };
                    if (createdByUserDetails.UserRoleId == 1)
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.CountryMajorMappingBySuperAdminNotificationTitleTemplate, minorIndustry.MinorIndustryName);
                        notification.NotificationMessage = string.Format(ApiConstants.CountryMajorMappingBySuperAdminNotificationMessageTemplate, minorIndustry.MinorIndustryName);
                    }
                    else
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
                        notification.NotificationTitle = string.Format(ApiConstants.CountryMajorMappingNotificationTitleTemplate, createdByUserDetails.UserName, minorIndustry.MinorIndustryName);
                        notification.NotificationMessage = string.Format(ApiConstants.CountryMajorMappingNotificationMessageTemplate, createdByUserDetails.UserName, minorIndustry.MinorIndustryName);
                    }

                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddMajorIndustryApprovalSuccessNotification(long majorIndustryId, string approveStatus)
        {
            try
            {
                var majorIndusryDetails = await _dbContext.MajorIndustries.FindAsync(majorIndustryId);
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(majorIndusryDetails.CreatedBy.Value);
                if (createdByUserDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.ManagerId,
                        SenderUserName = createdByUserDetails.ManagerName,
                        RecipientUserId = createdByUserDetails.UserId,
                        RecipientUserName = createdByUserDetails.UserName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.MajorIndustry
                    };
                    if (approveStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMajorIndustryApprovedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMajorIndustryApprovedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else if (approveStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMajorIndustryRejectedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMajorIndustryRejectedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMajorIndustryForwardedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMajorIndustryForwardedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddMinorIndustryApprovalSuccessNotification(long minorIndustryId, string approveStatus)
        {
            try
            {
                var minorIndustryDetails = await _dbContext.MinorIndustries.FindAsync(minorIndustryId);
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(minorIndustryDetails.CreatedBy.Value);
                if (createdByUserDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.ManagerId,
                        SenderUserName = createdByUserDetails.ManagerName,
                        RecipientUserId = createdByUserDetails.UserId,
                        RecipientUserName = createdByUserDetails.UserName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.MajorIndustry
                    };
                    if (approveStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMinorIndustryApprovedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMinorIndustryApprovedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else if (approveStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMinorIndustryRejectedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMinorIndustryRejectedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Forward;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMinorIndustryForwardedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMinorIndustryForwardedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddMinorIndustryMappingApprovalSuccessNotification(long minorIndustryId, string approveStatus)
        {
            try
            {
                var minorIndustryDetails = await _dbContext.MinorIndustries.FindAsync(minorIndustryId);
                var createdByUserDetails = await _helperRepository.GetUserAndManagerInfoAsync(minorIndustryDetails.CreatedBy.Value);
                if (createdByUserDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.ManagerId,
                        SenderUserName = createdByUserDetails.ManagerName,
                        RecipientUserId = createdByUserDetails.UserId,
                        RecipientUserName = createdByUserDetails.UserName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.MajorIndustry
                    };
                    if (approveStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMinorIndustryMappingApprovedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMinorIndustryMappingApprovedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    else if (approveStatus == Models.Enums.RefApprovalStatus.Rejected.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = string.Format(ApiConstants.NewMinorIndustryMappingRejectedNotificationTitleTemplate, createdByUserDetails.ManagerName);
                        notification.NotificationMessage = string.Format(ApiConstants.NewMinorIndustryMappingRejectedNotificationMessageTemplate, createdByUserDetails.ManagerName);
                    }
                    await _dbContext.AddAsync(notification);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<string> GetNextMinorIndustryCode()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[MinorIndustry]");
                int nextNumber = count + 1;
                return $"{ReferencePrefixes.MinorIndustry}{nextNumber:D3}";
            }
        }

        public async Task<string> GetNextMajorIndustryCode()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[MajorIndustry]");
                int nextNumber = count + 1;
                return $"{ReferencePrefixes.MajorIndustry}{nextNumber:D3}";
            }
        }

    }
}
