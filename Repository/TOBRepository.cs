using ComplianceAPI.Helpers;
using ComplianceAPI.Helpers.Constants;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Models.Enums;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace ComplianceAPI.Repository
{
    public interface ITOBRepository
    {
        Task<TOB> GetTOBHistory(long HistoryId);
        Task<List<TOBDetails>> GetAllTOB();
        Task<bool> PostUpdateTOBApproval(AccessModel access);
        Task<List<TOBMapping>> GetTOBMinorIndusryMapping(int tobId);
        Task<List<PendingApproval>> GetPendingTOBApproval(Guid? UserUID);
        Task<List<TOBApprovalList>> GetTOBMappingApprovaList(Guid UserUID);
        Task<List<TOBMappingList>> GetTOBMappingList();
        Task<List<TOBMappingList>> GetTOBMappingByCountry(long? countryId);
        Task<List<TOBMappingList>> GetTOBMappingByMajor(long? majorInudustryId, long? countryId);
        Task<List<TOBMappingList>> GetTOBMappingByMinor(long? minorInudustryId, long? majorInudustryId, int? countryId);
        Task<TOB> GetHistoryTOB(Guid? Uid);
        Task<TOB> AdminAddTOB(AddTOB TOB);
        Task<TOB> AddTOBDetails(TOB tobDetails);
        Task<TOB> AddUpdateTOBAsync(TOB tob, bool isSuperAdmin);
        Task<TOBMappingList> PostTOBMapping(TOBMappingList tobMapping);
        Task<bool> PostTOBApprovalMapping(AccessModel access, string ApprovalStatus);
        Task<bool> PostTOBApproveAccess(AccessModel access);
        Task<TOB> ApproveTOB(AccessModel access);
        Task<TOB> RejectTOB(AccessModel access);
        Task<bool> AddTOBApprovalNotification(long createdBy, string typeOfBranch);
        Task<bool> AddTOBMappingApprovalNotification(int createdBy, long? tobId);
        Task<string> GetNextTOBCode();

    }
    public class TOBRepository : ITOBRepository
    {
        private readonly ComplianceDbContext dbContext;
        private readonly IHelperRepository helperRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TOBRepository(ComplianceDbContext dbContext, IHelperRepository helperRepository, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;
            this.helperRepository = helperRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TOB> GetTOBHistory(long HistoryId)
        {

            var parameter = (from ph in dbContext.TOBHistory
                             join ud in dbContext.Users on ph.ManagerId equals ud.Id into managerJoin
                             where ph.HistoryId == HistoryId
                             select new TOB
                             {
                                 HistoryId = ph.HistoryId,
                                 Id = Convert.ToByte(ph.TOBId),
                                 TOBName = ph.TOBName,
                                 Status = ph.Status,
                                 ManagerId = ph.ManagerId,
                                 CreatedOn = ph.CreatedOn,
                                 CreatedBy = ph.CreatedBy,
                                 ModifiedBy = ph.ModifiedBy,
                                 ModifiedOn = ph.ModifiedOn,
                                 UID = ph.UID,
                                 //ManagerName = manager.FullName
                             }).FirstOrDefault();

            return parameter;
        }
        public async Task<List<TOBDetails>> GetAllTOB()
        {
            var res = await dbContext.TOBDetails.Where(x => x.Status == 1).ToListAsync();
            return res.OrderByDescending(x => x.CreatedOn).ToList();
        }
        public async Task<TOB> AdminAddTOB(AddTOB TOB)
        {
            int superAdminCount = 0;
            string roleName = "";
            string message = "Successfully Saved";
            ComplianceAPI.Models.DataModels.TOBDetails tobobject = null;
            tobobject = new ComplianceAPI.Models.DataModels.TOBDetails
            {
                //EmpId = TOB.CreatedBy.ToString(),
                TOBName = TOB.TOBName,
                Status = 0,
                ManagerId = TOB.ManagerId,
                CreatedOn = DateTime.Now,
                CreatedBy = TOB.CreatedBy,

            };
            dbContext.TOBDetails.Add(tobobject);
            dbContext.SaveChanges();

            return dbContext.TOBDetails
                .Where(p => p.Id == tobobject.Id)
                .Select(p => new TOB
                {
                    Id = p.Id,
                    TOBName = p.TOBName,
                    Status = Convert.ToByte(p.Status),
                    ManagerId = p.ManagerId,
                    CreatedOn = p.CreatedOn,
                    CreatedBy = p.CreatedBy,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn,
                    UID = p.UID,
                    ResponseCode = 1,
                    ResponseMessage = message
                })
                .FirstOrDefault();

        }
        public async Task<List<TOBMapping>> GetTOBMinorIndusryMapping(int tobId)
        {
            var result = dbContext.TOBMapping.Where(x => x.TOBId == tobId && x.Status == '1').ToList();
            return result;
        }
        public async Task<TOBMappingList> PostTOBMapping(TOBMappingList tobMapping)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                var mul = await sqlContext.Connection.QueryFirstAsync<TOBMappingList>("[product_owner].USP_POSTTOBMAPPING", new
                {
                    Id = tobMapping.Id,
                    TOBId = tobMapping.TOBId,
                    CountryId = tobMapping.CountryId,
                    MajorIndustryId = tobMapping.MajorIndustryId,
                    MinorIndustryId = tobMapping.MinorIndustryId,
                    CreatedBy = tobMapping.CreatedBy,
                    UID = tobMapping.UID
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                
                return mul;
            }
        }
        public async Task<bool> PostTOBApprovalMapping(AccessModel access, string ApprovalStatus)
        {
            using (var sqlContext = _unitOfWork.ContextFactory())
            {
                await sqlContext.Connection.QueryAsync<object>("[product_owner].USP_UPDATETOBMAPPINGAPPROVAL", new
                {
                    TOBMappingId = access.TOBMappingId,
                    ManagerId = access.ManagerId,
                    CreatedBy = access.CreatedBy,
                    UID = access.UID,
                    ApprovalStatus = ApprovalStatus,
                }, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlContext.Transaction).ConfigureAwait(false);
                sqlContext.Commit();
                if (access.UID != null)
                {
                    await TOBMappingApprovalResponseNotification(access.UID.Value, ApprovalStatus);
                }
                
                return true;
            }
        }
        public async Task<List<PendingApproval>> GetPendingTOBApproval(Guid? UserUID)
        {
            try
            {
                using (var sqlContext = _unitOfWork.ConnectionFactory())
                {

                    var mul = await sqlContext.QueryMultipleAsync("[product_owner].[USP_GET_TOB_APPROVAL_LIST]", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                    var Result = mul.Read<PendingApproval>().ToList();
                    mul.Dispose();
                    return Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TOBMappingList>> GetTOBMappingList()
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETTOBMAPPING", commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<TOBMappingList>();
                mul.Dispose();
                return Result.ToList();
            }

        }
        public async Task<List<TOBMappingList>> GetTOBMappingByCountry(long? countryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETTOBMAPPINGBYCOUNTRY", new { CountryId = countryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<TOBMappingList>();
                mul.Dispose();
                return Result.DistinctBy(x => x.MajorIndustryName).ToList();
            }
        }
        public async Task<List<TOBMappingList>> GetTOBMappingByMajor(long? majorInudustryId, long? countryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETTOBMAPPINGBYMAJOR", new { MajorIndustryId = majorInudustryId, CountryId = countryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<TOBMappingList>();
                mul.Dispose();
                return Result.DistinctBy(x => x.MinorIndustryName).ToList();
            }

        }
        public async Task<List<TOBMappingList>> GetTOBMappingByMinor(long? minorInudustryId, long? majorInudustryId, int? countryId)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETTOBMAPPINGBYMINOR", new { MinorIndustryId = minorInudustryId, MajorIndustryId = majorInudustryId, CountryId = countryId }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<TOBMappingList>();
                mul.Dispose();
                return Result.ToList();
            }
            //try
            //{
            //    var accessresult = await dbContext.TOBMapping.Where(a => a.MajorIndustryId == majorInudustryId && a.MinorIndustryId == minorInudustryId && a.CountryId == countryId && a.Status == 1).ToListAsync();
            //    return accessresult;
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}

        }
        public async Task<List<TOBApprovalList>> GetTOBMappingApprovaList(Guid UserUID)
        {
            using (var sqlContext = _unitOfWork.ConnectionFactory())
            {
                var mul = await sqlContext.QueryMultipleAsync("[product_owner].USP_GETTOBMAPPINGAPPROVALLIST", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                var Result = mul.Read<TOBApprovalList>();
                mul.Dispose();
                return Result.ToList();
            }
        }
        public async Task<TOB> GetHistoryTOB(Guid? Uid)
        {
            var approval = await dbContext.TOBApproval.Where(f => f.UID == Uid).FirstOrDefaultAsync();
            var result = (from tobhistory in dbContext.TOBHistory
                          where tobhistory.HistoryId == approval!.HistoryId
                          select new TOB
                          {
                              HistoryId = tobhistory.HistoryId,
                              TOBName = tobhistory.TOBName,
                              Status = tobhistory.Status,
                              ManagerId = tobhistory.ManagerId,
                              CreatedOn = tobhistory.CreatedOn,
                              CreatedBy = tobhistory.CreatedBy,
                              ModifiedBy = tobhistory.ModifiedBy,
                              ModifiedOn = tobhistory.ModifiedOn,
                              UID = tobhistory.UID,
                          }).FirstOrDefault();

            return result;
        }
        public async Task<TOB> AddTOBDetails(TOB tobDetails)
        {
            //int superAdminCount = 0;
            //string roleName = "";
            //string message = "Successfully Saved";
            //ComplianceAPI.Models.DataModels.RegulationSetupHistory regulationsetupdetailsobject = null;
            try
            {
                var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)tobDetails.CreatedBy);
                //var stateList = await dbContext.CountryStateMapping.Where(x => x.CountryId == tobDetails).ToListAsync();
                if (isSuperAdmin)
                {
                    await AddUpdateTOBAsync(tobDetails, isSuperAdmin);
                }
                var request = new TOBHistory
                {
                    //EmpId = regulationStupDetails.CreatedBy.ToString(),
                    TOBName = tobDetails.TOBName,
                    Status = 0,
                    //ManagerId = regulationStupDetails.ManagerId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = tobDetails.CreatedBy,
                };

                dbContext.TOBHistory.Add(request);
                await dbContext.SaveChangesAsync();

                return new TOB() { HistoryId = request.HistoryId, ResponseCode = 1, ResponseMessage = isSuperAdmin ? "Saved Successfully" : "Request sent for Approval",TOBReferenceCode = tobDetails.TOBReferenceCode };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<TOB> AddUpdateTOBAsync(TOB tob, bool isSuperAdmin)
        {
            // Assuming `compliance` is the object containing the data to be saved or updated
            var existingTOB = await dbContext.TOBDetails
                .FirstOrDefaultAsync(c => c.UID == tob.UID || c.Id == tob.Id);

            if (existingTOB != null)
            {
                // Update existing recordGetRegSetupComplianceHistory
                existingTOB.TOBName = tob.TOBName;
                existingTOB.Status = isSuperAdmin ? 1 : 0;
                existingTOB.ModifiedOn = DateTime.Now;
                existingTOB.ModifiedBy = tob.ModifiedBy;
            }
            else
            {
                // Add new record
                var Adminrequest = new TOBDetails
                {
                    TOBName = tob.TOBName,
                    CreatedOn = DateTime.Now,
                    CreatedBy = tob.CreatedBy,
                    Status = isSuperAdmin ? 1 : 0,
                    TOBReferenceCode = tob.TOBReferenceCode
                };
                dbContext.TOBDetails.Add(Adminrequest);
                await dbContext.SaveChangesAsync();
            }

            await dbContext.SaveChangesAsync();

            return tob;

        }
        public async Task<bool> PostTOBApproveAccess(AccessModel access)
        {

            var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)access.CreatedBy);

            var statusIds = dbContext.RefApprovalStatus
                            .Where(s => s.Status == "Approved" || s.Status == "Reviewed" || s.Status == "Pending")
                            .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = statusIds.GetValueOrDefault("Approved");
            var reviewerStatusId = statusIds.GetValueOrDefault("Reviewed");
            var pendingApprovalId = statusIds.GetValueOrDefault("Pending");

            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Approved.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isSuperAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.TOBApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.TOBApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.TOBApproval.Add(new TOBApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    //ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var userApproval = dbContext.TOBApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == approvedStatusId) && isSuperAdmin))
            {
                var tob = dbContext.TOBHistory.FirstOrDefault(u => u.HistoryId == access.HistoryId);
                if (tob != null)
                {
                    ComplianceAPI.Models.DataModels.TOBDetails tobobject = null;
                    if (tob.HistoryId == 0 || tob.HistoryId == null)
                    {

                        tobobject = new ComplianceAPI.Models.DataModels.TOBDetails
                        {
                            //EmpId = regulation.EmpId,
                            TOBName = tob.TOBName,
                            Status = 1,
                            //ManagerId = regulation.ManagerId,
                            CreatedOn = DateTime.Now,
                            CreatedBy = tob.CreatedBy,
                        };
                        dbContext.TOBDetails.Add(tobobject);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        tobobject = dbContext.TOBDetails.FirstOrDefault(u => u.Id == tob.HistoryId);
                        if (tob != null)
                        {
                            tobobject.TOBName = tob.TOBName;
                            //regulationSetuprobject.EmpId = regulation.EmpId;
                            //regulationSetuprobject.ManagerId = regulation.ManagerId;
                            tobobject.ModifiedBy = tob.CreatedBy;
                            tobobject.ModifiedOn = DateTime.Now;
                            tobobject.Status = 1;
                            dbContext.Update(tobobject);
                            // dbContext.SaveChanges();

                        }
                    }
                }
            }

            dbContext.SaveChanges();
            return true;
        }
        public async Task<bool> PostUpdateTOBApproval(AccessModel access)
        {

            var isSuperAdmin = helperRepository.IsSuperAdmin((Int64)access.CreatedBy);

            var approvalStatuses = dbContext.RefApprovalStatus
                                    .Where(s => new[] { "Approved", "Reviewed", "Pending", "Forward" }.Contains(s.Status))
                                    .ToDictionary(s => s.Status, s => s.Id);

            var approvedStatusId = approvalStatuses.GetValueOrDefault("Approved");
            var reviewerStatusId = approvalStatuses.GetValueOrDefault("Reviewed");
            var pendingApprovalId = approvalStatuses.GetValueOrDefault("Pending");
            var forwardId = approvalStatuses.GetValueOrDefault("Forward");


            var approvalStatusId = dbContext.RefApprovalStatus.Where(s => s.Status == RefApprovalStatusU.Pending.ToString())
                .Select(s => s.Id)
                .FirstOrDefault();

            var newManagerId = access.ManagerId ?? 0;

            if (isSuperAdmin)
            {
                if (approvalStatusId == approvedStatusId || approvalStatusId == pendingApprovalId)
                {
                    approvalStatusId = approvedStatusId;
                }
            }

            if (dbContext.TOBApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.TOBApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.TOBApproval.Add(new TOBApproval
                {
                    //UserId = access.UserId,
                    ManagerId = access.ManagerId,
                    //ApprovalType = access.ApprovalTypeId,
                    ApprovalStatus = approvalStatusId,
                    CreatedBy = access.CreatedBy,
                    CreatedOn = DateTime.Now,
                    HistoryId = access.HistoryId,
                    UID = Guid.NewGuid(),

                });
            }
            else
            {
                var userApproval = dbContext.TOBApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isSuperAdmin))
            {
                var user = dbContext.TOBDetails.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = 1;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            };

            dbContext.SaveChanges();
            return true;
        }
        public async Task<TOB> ApproveTOB(AccessModel access)
        {
            var tob = await this.GetHistoryTOB(access.UID);
            await AddUpdateTOBAsync(tob, true);
            var approval = dbContext.TOBApproval.Where(f => f.UID == access.UID).FirstOrDefault();

            approval.ApprovalStatus = Helpers.RefApprovalStatus.Approved;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.CreatedBy;
            dbContext.TOBApproval.Update(approval);

            await dbContext.SaveChangesAsync();
            await TOBApprovalResponseNotification(approval.CreatedBy.Value, RefApprovalStatusU.Approved);
            return new TOB() { ResponseCode = 1, ResponseMessage = "Approved Successfully" };
        }
        public async Task<TOB> RejectTOB(AccessModel access)
        {
            var approval = dbContext.TOBApproval.Where(f => f.UID == access.UID).FirstOrDefault();
            approval.ApprovalStatus = Helpers.RefApprovalStatus.Rejected;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.CreatedBy;
            dbContext.TOBApproval.Update(approval);

            await dbContext.SaveChangesAsync();
            await TOBApprovalResponseNotification(approval.CreatedBy.Value, RefApprovalStatusU.Rejected);
            return new TOB() { ResponseCode = 1, ResponseMessage = "Rejected Successfully" };
        }

        public async Task<bool> AddTOBApprovalNotification(long createdBy, string typeOfBranch)
        {
            try
            {
                var createdByUserDetails = await helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (createdByUserDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdByUserDetails.UserId,
                        SenderUserName = createdByUserDetails.UserName,
                        RecipientUserId = createdByUserDetails.ManagerId,
                        RecipientUserName = createdByUserDetails.ManagerName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.TypeOfBranch
                    };
                    if (createdByUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewTOBBySuperAdminTitleTemplate, typeOfBranch);
                        notification.NotificationMessage = string.Format(ApiConstants.NewTOBBySuperAdminMessageTemplate, typeOfBranch);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.NewTOBTitleTemplate, createdByUserDetails.UserName, typeOfBranch);
                        notification.NotificationMessage = string.Format(ApiConstants.NewTOBMessageTemplate, createdByUserDetails.UserName, typeOfBranch);
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
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

        public async Task<bool> AddTOBMappingApprovalNotification(int createdBy, long? tobId)
        {
            try
            {
                var createdBYUserDetails = await helperRepository.GetUserAndManagerInfoAsync(createdBy);
                var TOBDetailes = await dbContext.TOBDetails.FindAsync(tobId);
                if(createdBYUserDetails !=null && TOBDetailes != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = createdBYUserDetails.UserId,
                        SenderUserName = createdBYUserDetails.UserName,
                        RecipientUserId = createdBYUserDetails.ManagerId,
                        RecipientUserName = createdBYUserDetails.ManagerName,
                        ModuleType = Models.Enums.ModuleType.TypeOfBranchMapping,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false
                    };
                    if (createdBYUserDetails.UserRoleId == 1)
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.TOBMappingBySuperAdminNotificationTitleTemplate, TOBDetailes.TOBName);
                        notification.NotificationMessage = string.Format(ApiConstants.TOBMappingBySuperAdminNotificationMessageTemplate, TOBDetailes.TOBName);
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                    }
                    else
                    {
                        notification.NotificationTitle = string.Format(ApiConstants.TOBMappingNotificationTitleTemplate, createdBYUserDetails.UserName, TOBDetailes.TOBName);
                        notification.NotificationMessage = string.Format(ApiConstants.TOBMappingNotificationMessageTemplate, createdBYUserDetails.UserName, TOBDetailes.TOBName);
                        notification.Status = Models.Enums.RefApprovalStatus.Pending;
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

        public async Task<bool> TOBApprovalResponseNotification(long createdBy, string approvalStatus)
        {
            try
            {
                var userDetails = await helperRepository.GetUserAndManagerInfoAsync(createdBy);
                if (userDetails != null)
                {
                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        SenderUserId = userDetails.ManagerId,
                        SenderUserName = userDetails.ManagerName,
                        RecipientUserId = userDetails.UserId,
                        RecipientUserName = userDetails.ManagerName,
                        CreatedDate = DateTime.UtcNow,
                        ReadDate = null,
                        MarkAsRead = false,
                        ModuleType = Models.Enums.ModuleType.Country
                    };
                    if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Approved;
                        notification.NotificationTitle = $"{userDetails.ManagerName} approved a <b>New TOB</b>.";
                        notification.NotificationMessage= $"{userDetails.ManagerName} approved a <b>New TOB</b>.";
                    }
                    else
                    {
                        notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                        notification.NotificationTitle = $"{userDetails.ManagerName} rejected the <b>New TOB</b>";
                        notification.NotificationMessage = $"{userDetails.ManagerName} rejected the <b>New TOB</b>";
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

        public async Task<bool> TOBMappingApprovalResponseNotification(Guid UID, string approvalStatus)
        {
            try
            {
                var tobMappingApprovalDetails = await dbContext.TOBMappingApproval.FirstOrDefaultAsync(x => x.UID == UID);
                if (tobMappingApprovalDetails != null)
                {
                    var userDetails = await helperRepository.GetUserAndManagerInfoAsync(tobMappingApprovalDetails.CreatedBy.Value);
                    if (userDetails != null)
                    {
                        var notification = new Notification
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            SenderUserId = userDetails.ManagerId,
                            SenderUserName = userDetails.ManagerName,
                            RecipientUserId = userDetails.UserId,
                            RecipientUserName = userDetails.UserName,
                            CreatedDate = DateTime.UtcNow,
                            ReadDate = null,
                            MarkAsRead = false,
                            ModuleType = Models.Enums.ModuleType.TypeOfBranchMapping
                        };
                        if (approvalStatus == Models.Enums.RefApprovalStatus.Approved.ToString())
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Approved;
                            notification.NotificationTitle = $"{userDetails.ManagerName} approved a <b>New TOB Mapping</b>.";
                            notification.NotificationMessage = $"{userDetails.ManagerName} approved a <b>New TOB Mapping</b>.";
                        }
                        else
                        {
                            notification.Status = Models.Enums.RefApprovalStatus.Rejected;
                            notification.NotificationTitle = $"{userDetails.ManagerName} rejected the <b>New TOB Mapping</b>";
                            notification.NotificationMessage = $"{userDetails.ManagerName} rejected the <b>New TOB Mapping</b>";
                        }
                        await dbContext.AddAsync(notification);
                        await dbContext.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<string> GetNextTOBCode()
        {
            using (var connection = _unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM [product_owner].[TOB]");
                int nextNumber = count + 1;
                return $"{ReferencePrefixes.TOB}{nextNumber.ToString("D3")}";
            }
        }

    }
}
