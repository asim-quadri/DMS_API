using ComplianceAPI.Models.Enums;
using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Repository;
using RefApprovalType = ComplianceAPI.Helpers.RefApprovalType;

namespace ComplianceAPI.Services
{
    public interface IAnnouncementService
    {
        Task<Announcements> GetAnnouncementDetails(long? Id);
        Task<List<AnnouncementApprovalModal>> GetPendingAnnouncementApproval(Guid? UserUID);
        Task<Announcement> GetHistoryAnnouncement(Guid? uid);
        Task<Announcement> AddAnnouncementDetails(Announcement announcement);
        Task<Announcement> ApproveAnnouncement(AccessModel access);
        Task<Announcement> RejectAnnouncement(AccessModel access);

        Task<string> GetNextAnnouncementReferenceCode();

        Task<List<Announcement>> GetAllAnnouncement(long? Id, string? tocruletype);
    }
    public class AnnouncementService: IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementpRepository;
        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementpRepository = announcementRepository;
        }
        public async Task<Announcements> GetAnnouncementDetails(long? Id)
        {
            return await _announcementpRepository.GetAnnouncementDetails(Id);
        }
        public async Task<List<AnnouncementApprovalModal>> GetPendingAnnouncementApproval(Guid? UserUID)
        {
            return await _announcementpRepository.GetPendingAnnouncementApproval(UserUID);
        }
        public async Task<Announcement> GetHistoryAnnouncement(Guid? uid)
        {
            return await _announcementpRepository.GetHistoryAnnouncement(uid);
        }
        public async Task<Announcement> ApproveAnnouncement(AccessModel access)
        {
            return await _announcementpRepository.ApproveAnnouncement(access);
        }
        public async Task<Announcement> RejectAnnouncement(AccessModel access)
        {
            return await _announcementpRepository.RejectAnnouncement(access);
        }
        public async Task<Announcement> AddAnnouncementDetails(Announcement announcement)
        {
            var result = await _announcementpRepository.AddAnnouncementDetails(announcement);
            AccessModel access = new AccessModel() { ApprovalTypeId = RefApprovalType.User, CreatedBy = Convert.ToByte(announcement.CreatedBy), ManagerId = announcement.ApprovalManagerId == null ? 0 : announcement.ApprovalManagerId, Status = 0, UserId = result.Id, HistoryId = result.HistoryId };
            await _announcementpRepository.PostUpdateAnnouncementApproval(access);
            return result;
        }

        public async Task<List<Announcement>> GetAllAnnouncement(long? Id, string? tocruletype)
        {
            return await _announcementpRepository.GetAllAnnouncementDetailsAsync(Id, tocruletype);
        }

        public async Task<string> GetNextAnnouncementReferenceCode()
        {
            return await _announcementpRepository.GetNextAnnouncementReferenceCode();
        }


    }
}
