using ComplianceAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("GetAllApprovalNotifications")]
        public async Task<IActionResult> GetAllApprovalNotifications(long userId)
        {
            try
            {
                var result = await _notificationService.GetAllApprovalNotifications(userId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("MarkNotificationAsRead")]
        public async Task<IActionResult> MarkNotificationAsRead(Guid? UserUID, Guid? notificationId, bool markAsRead)
        {
            try
            {
                var result = await _notificationService.MarkNotificationAsRead(UserUID, notificationId, markAsRead);
                if (result)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}