using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers.ProductOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterSetupController : ControllerBase
    {
        private readonly IParameterService _parameterService;
        public ParameterSetupController(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }

        [HttpGet]
        [Route("GetAllParameters")]
        public async Task<ActionResult> GetAllParameters()
        {
            try
            {
                var obj = await _parameterService.GetAllParameters();
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
        [Route("GetHistoryParameters/{HistoryId}")]
        public async Task<ActionResult> GetHistoryParameters(long HistoryId)
        {
            var obj = await _parameterService.GetHistoryParameters(HistoryId);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpPost]
        [Route("PostApproveAccess")]
        public async Task<ActionResult> PostApproveAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _parameterService.PostParameterApproveAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostRejectAccess")]
        public async Task<ActionResult> PostRejectAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _parameterService.PostParameterRejectAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostReviewedAccess")]
        public async Task<ActionResult> PostReviewedAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _parameterService.PostParameterReviewAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddParameter")]
        public async Task<ActionResult> AddParameter([FromBody] AddParameter parameter, Guid? accessUID)
        {
            try
            {
                var obj = await _parameterService.AddParameter(parameter, accessUID);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetParameterById/{id}")]
        public async Task<ActionResult> GetParameterById(long id)
        {
            try
            {
                var obj = await _parameterService.GetParameterById(id);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        [Route("deleteParameters/{uid}/{status}")]
        public async Task<ActionResult> deleteParameters(Guid uid, int status)
        {
            var obj = await _parameterService.DeleteParameter(uid, status);
            if (obj != null)
                return Ok(obj);
            return NotFound();

        }

        [HttpGet]
        [Route("GetAllParametersApproval")]
        public async Task<ActionResult> GetAllParametersApproval()
        {
            try
            {
                var obj = await _parameterService.GetAllParameterApproval();
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
        [Route("GetAllParametersApproval/{UserUID}")]
        public async Task<ActionResult> GetAllParametersApproval(Guid? UserUID)
        {
            try
            {
                var obj = await _parameterService.GetPendingParameterApproval(UserUID);
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
        [Route("GetNextParameterCode")]
        public async Task<ActionResult> GetNextParameterCode()
        {
            try
            {
                var code = await _parameterService.GetNextParameterCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
