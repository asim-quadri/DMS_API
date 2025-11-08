using ComplianceAPI.Models;
using ComplianceAPI.Models.Enums;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        [Route("GetAnnouncementDetails")]
        public async Task<ActionResult> GetAnnouncementDetails(long? Id)
        {
            try
            {
                var obj = await _announcementService.GetAnnouncementDetails(Id);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetPendingAnnouncementApproval/{UserUID}")]
        public async Task<ActionResult> GetPendingAnnouncementApproval(Guid? UserUID)
        {
            var obj = await _announcementService.GetPendingAnnouncementApproval(UserUID);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpGet]
        [Route("GetHistoryAnnouncement/{UID}")]
        public async Task<ActionResult> GetHistoryAnnouncement(Guid? UID)
        {
            var obj = await _announcementService.GetHistoryAnnouncement(UID);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpPost]
        [Route("AddAnnouncementDetails")]
        public async Task<ActionResult> AddAnnouncementDetails([FromBody] Announcement announcement)
        {
            try
            {
                var obj = await _announcementService.AddAnnouncementDetails(announcement);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ApproveAnnouncement")]
        public async Task<ActionResult> ApproveAnnouncement([FromBody] AccessModel access)
        {
            var obj = await _announcementService.ApproveAnnouncement(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpPost]
        [Route("RejectAnnouncement")]
        public async Task<ActionResult> RejectAnnouncement([FromBody] AccessModel access)
        {
            var obj = await _announcementService.RejectAnnouncement(access);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpGet]
        [Route("GetAllAnnouncement")]
        public async Task<ActionResult> GetAnnouncements(long? id, string? ruleType)
        {
            try
            {
                var obj = await _announcementService.GetAllAnnouncement(id, ruleType);
                if (obj != null)
                {

                    return Ok(obj);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetNextAnnouncementsReferenceCode")]
        public async Task<ActionResult> GetNextAnnouncementsReference()
        {
            var obj = await _announcementService.GetNextAnnouncementReferenceCode();
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

    }
}
