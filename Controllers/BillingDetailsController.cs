using ComplianceAPI.Models;
using ComplianceAPI.Models.DataModels;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingDetailsController : Controller
    {

        private readonly IBillingService _billingService;
        public BillingDetailsController(IBillingService billingService)
        {
            _billingService = billingService;
        }
        [HttpGet]
        [Route("GetAllBillingLevel")]
        public async Task<ActionResult> GetAllBillingLevel()
        {
            try
            {
                var obj = await _billingService.GetAllBillingLevel();
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
        [Route("GetAllBillingFrequency")]
        public async Task<ActionResult> GetAllBillingFrequency()
        {
            try
            {
                var obj = await _billingService.GetAllBillingFrequency();
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
        [Route("GetAllServiceProvider")]
        public async Task<ActionResult> GetAllServiceProvider()
        {
            try
            {
                var obj = await _billingService.GetAllServiceProvider();
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
        [Route("GetAllBillStatus")]
        public async Task<ActionResult> GetAllBillStatus()
        {
            try
            {
                var obj = await _billingService.GetAllBillStatus();
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
        [Route("GetAllDeliveryStatus")]
        public async Task<ActionResult> GetAllDeliveryStatus()
        {
            try
            {
                var obj = await _billingService.GetAllDeliveryStatus();
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("PostBillingDetails")]
        public async Task<ActionResult> PostBillingDetails([FromBody] PostBillingDetails billingDetails)
        {
            try
            {
                var obj = await _billingService.PostBillingDetails(billingDetails);
                if (obj)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBillingDetailsView")]
        public async Task<ActionResult> GetBillingDetailsView(long billingDetailId)
        {
            try
            {
                var obj = await _billingService.GetBillingDetailsView(billingDetailId);
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
        [Route("GetBillingDetailsViewByOrgId")]
        public async Task<ActionResult> GetBillingDetailsViewByOrgId(long organizationId)
        {
            try
            {
                var obj = await _billingService.GetBillingDetailsViewByOrgId(organizationId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostBillingApprove")]
        public async Task<ActionResult> PostBillingApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _billingService.PostBillingApprove(access);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostBillingReject")]
        public async Task<ActionResult> PostBillingReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _billingService.PostBillingReject(access);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostBillingForward")]
        public async Task<ActionResult> PostBillingForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _billingService.PostBillingForward(access);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBillingDetailsByEntityAsync")]
        [ProducesResponseType(typeof(List<BillingDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBillingDetailsByEntityAsync([FromQuery] long? entityId)
        {
            var details = await _billingService.GetBillingDetailsByEntityAsync(entityId);
            return Ok(details);
        }
    }
}
