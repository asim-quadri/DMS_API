using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using RefApprovalStatus = ComplianceAPI.Helpers.RefApprovalStatus;

namespace ComplianceAPI.Repository
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> AddAnnouncementDetails(Announcement announcement);
        Task<Announcement> AddUpdateAnnouncementDetailsAsync(Announcement announcement, bool isSuperAdmin);
        Task<Announcement> ApproveAnnouncement(AccessModel access);
        Task<Announcement> RejectAnnouncement(AccessModel access);
        Task<bool> PostUpdateAnnouncementApproval(AccessModel access);
        Task<Announcements> GetAnnouncementDetails(long? Id);
        Task<List<AnnouncementApprovalModal>> GetPendingAnnouncementApproval(Guid? UserUID);
        Task<Announcement> GetHistoryAnnouncement(Guid? uid);

        Task<string> GetNextAnnouncementReferenceCode();
        Task<List<Announcement>> GetAllAnnouncementDetailsAsync(long? id, string? ruleType);
    }
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ComplianceDbContext dbContext;
        private readonly IHelperRepository helperRepository;
        private readonly IUnitOfWork unitOfWork;

        public AnnouncementRepository(ComplianceDbContext dbContext, IHelperRepository helperRepository, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;
            this.helperRepository = helperRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Announcements> GetAnnouncementDetails(long? Id)
        {
            try
            {
                //if (Id == null)
                //    return null;

                var result = dbContext.Announcements.Where(x => x.Id == Id).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Announcement> GetHistoryAnnouncement(Guid? uid)
        {
            var approval = await dbContext.AnnouncementApproval.Where(a => a.UID == uid).FirstOrDefaultAsync();
            var result = await dbContext.AnnouncementHistory.Where(r => r.HistoryId == approval!.HistoryId).FirstOrDefaultAsync();

            if (result == null)
                return null;

            var announcement = new Announcement
            {
                HistoryId = result.HistoryId,
                RegulationId = result.RegulationId,
                ApplicableDate = result.ApplicableDate,
                Description = result.Description,
                ApprovalManagerId = Convert.ToInt32(result.ManagerId),
                Status = Convert.ToInt32(result.Status),
                CreatedDate = result.CreatedDate,
                CreatedBy = (long)result.CreatedBy,
                createdByName = result.createdByName,
                Subject = result.Subject,
                AnnouncementReferencecode = result.AnnouncementReferencecode,
                ComplianceId = result.ComplainceId,
                RegisterId = result.RegisterationId,
                TOCId = result.TOCId
            };

            return announcement;
        }
        public async Task<List<AnnouncementApprovalModal>> GetPendingAnnouncementApproval(Guid? UserUID)
        {
            try
            {
                using (var sqlContext = unitOfWork.ConnectionFactory())
                {
                    var mul = await sqlContext.QueryMultipleAsync("[product_owner].[USP_GET_ANNOUNCEMENT_APPROVAL_LIST]", new { UserUID = UserUID }, commandType: System.Data.CommandType.StoredProcedure);
                    var Result = mul.Read<AnnouncementApprovalModal>().ToList();
                    mul.Dispose();
                    return Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Announcement> AddAnnouncementDetails(Announcement announcement)
        {
            var isSuperAdmin = helperRepository.IsSuperAdmin(announcement.CreatedBy);
            if (isSuperAdmin)
            {
                await AddUpdateAnnouncementDetailsAsync(announcement, isSuperAdmin);
            }
            AnnouncementHistory existingHistory = null;
            if (announcement.HistoryId != null && announcement.HistoryId > 0)
            {
                existingHistory = await dbContext.AnnouncementHistory
                    .FirstOrDefaultAsync(h => h.HistoryId == announcement.HistoryId);
            }

            if (existingHistory != null)
            {
                // Update existing announcement history
                existingHistory.RegulationId = announcement.RegulationId;
                existingHistory.CreatedDate = announcement.CreatedDate;
                existingHistory.CreatedBy = announcement.CreatedBy;
                existingHistory.ApplicableDate = announcement.ApplicableDate;
                existingHistory.Description = announcement.Description;
                existingHistory.Subject = announcement.Subject;
                existingHistory.ComplainceId = announcement.ComplianceId;
                existingHistory.createdByName = announcement.createdByName;
                existingHistory.AnnouncementReferencecode = announcement.AnnouncementReferencecode;
                existingHistory.Status = false;
                existingHistory.ModifiedOn = DateTime.Now;
                existingHistory.ModifiedBy = announcement.CreatedBy;
                existingHistory.TOCId = announcement.TOCId;
                existingHistory.RegisterationId = announcement.RegisterId;


                dbContext.AnnouncementHistory.Update(existingHistory);
                await dbContext.SaveChangesAsync();

                return new Announcement()
                {
                    HistoryId = existingHistory.HistoryId,
                    ResponseCode = 1,
                    ResponseMessage = "Updated Successfully"
                };
            }
            else
            {
                // Add new announcement history
                var request = new AnnouncementHistory
                {
                    RegulationId = announcement.RegulationId,
                    CreatedDate = announcement.CreatedDate,
                    CreatedBy = announcement.CreatedBy,
                    ApplicableDate = announcement.ApplicableDate,
                    Description = announcement.Description,
                    Status = false,
                    UID = Guid.NewGuid(),
                    Subject = announcement.Subject,
                    ComplainceId = announcement.ComplianceId,
                    createdByName = announcement.createdByName,
                    AnnouncementReferencecode = announcement.AnnouncementReferencecode,
                    RegisterationId = announcement.RegisterId,
                    TOCId = announcement.TOCId,
                };
                dbContext.AnnouncementHistory.Add(request);
                await dbContext.SaveChangesAsync();
                
                return new Announcement()
                {
                    HistoryId = request.HistoryId,
                    ResponseCode = 1,
                    ResponseMessage = isSuperAdmin ? "Saved Successfully" : "Request sent for Approval"
                };
            }
        }
        
        public async Task<Announcement> ApproveAnnouncement(AccessModel access)
        {
            long regulationid = 0;
            var announcements = await this.GetHistoryAnnouncement(access.UID);
            await AddUpdateAnnouncementDetailsAsync(announcements, true);
            var approval = dbContext.AnnouncementApproval.Where(f => f.UID == access.UID).FirstOrDefault();

            approval.ApprovalStatus = RefApprovalStatus.Approved;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.CreatedBy;
            dbContext.AnnouncementApproval.Update(approval);

            await dbContext.SaveChangesAsync();
            return new Announcement() { ResponseCode = 1, ResponseMessage = "Approved Successfully" };
        }
        public async Task<Announcement> RejectAnnouncement(AccessModel access)
        {
            var approval = dbContext.AnnouncementApproval.Where(f => f.UID == access.UID).FirstOrDefault();
            approval.ApprovalStatus = RefApprovalStatus.Rejected;
            approval.ModifiedOn = DateTime.Now;
            approval.ModifiedBy = access.CreatedBy;
            dbContext.AnnouncementApproval.Update(approval);

            await dbContext.SaveChangesAsync();
            return new Announcement() { ResponseCode = 1, ResponseMessage = "Rejected Successfully" };
        }
        public async Task<Announcement> AddUpdateAnnouncementDetailsAsync(Announcement announcement, bool isSuperAdmin)
        {
            // Check if announcement exists (by Id)
            Announcements existingAnnouncement = null;
            if (announcement.Id != null && announcement.Id > 0)
            {
                existingAnnouncement = await dbContext.Announcements
                    .FirstOrDefaultAsync(a => a.Id == announcement.Id);
            }

            if (existingAnnouncement != null)
            {
                // Update existing announcement
                existingAnnouncement.RegulationId = announcement.RegulationId;
                existingAnnouncement.ComplainceId = announcement.ComplianceId;
                existingAnnouncement.CreatedDate = announcement.CreatedDate;
                existingAnnouncement.CreatedBy = announcement.CreatedBy;
                existingAnnouncement.CreatedByName = announcement.createdByName;
                existingAnnouncement.ApplicableDate = announcement.ApplicableDate;
                existingAnnouncement.Description = announcement.Description;
                existingAnnouncement.Status = isSuperAdmin ? true : false;
                existingAnnouncement.RegisterationId = announcement.RegisterId;
                existingAnnouncement.TOCId = announcement.TOCId;
                existingAnnouncement.Subject = announcement.Subject;
                existingAnnouncement.AnnouncementReferencecode = announcement.AnnouncementReferencecode;
                existingAnnouncement.ModifiedOn = DateTime.Now;
                existingAnnouncement.ModifiedBy = announcement.CreatedBy;

                dbContext.Announcements.Update(existingAnnouncement);
                await dbContext.SaveChangesAsync();

                return announcement;
            }
            else
            {
                // Add new announcement
                var request = new Announcements
                {
                    RegulationId = announcement.RegulationId,
                    ComplainceId = announcement.ComplianceId,
                    CreatedDate = announcement.CreatedDate,
                    CreatedBy = announcement.CreatedBy,
                    CreatedByName = announcement.createdByName,
                    ApplicableDate = announcement.ApplicableDate,
                    Description = announcement.Description,
                    Status = isSuperAdmin ? true : false,
                    UID = Guid.NewGuid(),
                    Subject = announcement.Subject,
                    AnnouncementReferencecode = announcement.AnnouncementReferencecode,
                    RegisterationId = announcement.RegisterId,
                    TOCId = announcement.TOCId
                };
                dbContext.Announcements.Add(request);
                await dbContext.SaveChangesAsync();
                return announcement;
            }
        }
        
        public async Task<bool> PostUpdateAnnouncementApproval(AccessModel access)
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

            if (dbContext.AnnouncementApproval.Any(ua => ua.HistoryId == access.HistoryId && ua.ApprovalStatus == pendingApprovalId && ua.CreatedBy == access.CreatedBy))
            {
                return false;
            }

            if (!dbContext.AnnouncementApproval.Any(ua => ua.UID == access.UID))
            {
                dbContext.AnnouncementApproval.Add(new AnnouncementApproval
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
                var userApproval = dbContext.AnnouncementApproval.FirstOrDefault(ua => ua.UID == access.UID);
                if (userApproval != null)
                {
                    userApproval.ApprovalStatus = approvalStatusId;
                    userApproval.ModifiedBy = access.CreatedBy;
                    userApproval.ModifiedOn = DateTime.Now;
                }
            }

            if (approvalStatusId == approvedStatusId || ((approvalStatusId == forwardId || approvalStatusId == approvedStatusId) && isSuperAdmin))
            {
                var user = dbContext.Announcements.FirstOrDefault(u => u.Id == access.UserId);
                if (user != null)
                {
                    user.Status = true;
                    user.ModifiedBy = access.CreatedBy;
                    user.ModifiedOn = DateTime.Now;
                }
            }
            ;

            dbContext.SaveChanges();
            return true;
        }

        public async Task<List<Announcement>> GetAllAnnouncementDetailsAsync(long? id, string? ruleType)
        {
            var query = dbContext.Announcements.AsQueryable();

            if (id != null && !string.IsNullOrWhiteSpace(ruleType))
            {
                switch (ruleType.ToLower())
                {
                    case "regulation":
                        query = query.Where(a => a.RegulationId == id);
                        break;
                    case "compliance":
                        query = query.Where(a => a.ComplainceId == id);
                        break;
                    case "toc":
                        query = query.Where(a => a.TOCId == id && a.Status == true);
                        break;
                    case "registration":
                        query = query.Where(a => a.RegisterationId == id);
                        break;
                    default:
                        break;
                }
            }
            else if (id != null)
            {
                query = query.Where(a =>
                    a.RegulationId == id ||
                    a.ComplainceId == id ||
                    a.RegisterationId == id ||
                    (a.TOCId == id && a.Status == true)
                );
            }

            var result = await query
                .Select(a => new Announcement
                {
                    Id = a.Id,
                    RegulationId = a.RegulationId,
                    ApplicableDate = a.ApplicableDate,
                    Description = a.Description,
                    Status = Convert.ToInt32(a.Status),
                    CreatedDate = a.CreatedDate,
                    CreatedBy = a.CreatedBy ?? 0,
                    createdByName = a.CreatedByName,
                    Subject = a.Subject,
                    RegulationName = a.RegulationId != null
                        ? dbContext.RegulationSetupDetails
                            .Where(r => r.Id == a.RegulationId)
                            .Select(r => r.RegulationName)
                            .FirstOrDefault()
                        : null,
                    ComplianceId = a.ComplainceId,
                    RegisterId=a.RegisterationId,
                    TOCId=a.TOCId

                })
                .ToListAsync();

            return result;
        }

        public async Task<string> GetNextAnnouncementReferenceCode()
        {
            using (var connection = unitOfWork.ConnectionFactory())
            {
                var count = await connection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM [Announcements]");
                long nextNumber = count + 1;

                return nextNumber.ToString("D3");
            }
           
        }

    }
}
