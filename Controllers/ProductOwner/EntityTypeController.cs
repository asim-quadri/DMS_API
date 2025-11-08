using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityTypeController : ControllerBase
    {
        private readonly IEntityTypeService _EntityType;
        public EntityTypeController(IEntityTypeService EntityType)
        {
            _EntityType = EntityType;
        }

        [HttpGet]
        [Route("GetAllEntityTypes")]
        public async Task<ActionResult> GetAllEntityTypes()
        {
            try
            {
                var obj = await _EntityType.GetAllEntityTypes();
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
        [Route("GetEntityTypeByUID/{uid}")]
        public async Task<ActionResult> GetEntityTypeByUID(Guid uid)
        {
            try
            {
                var obj = await _EntityType.GetEntityTypeByUID(uid);
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
        [Route("PostEntityType")]
        public async Task<ActionResult> PostEntityType([FromBody] EntityTypeModel entityType)
        {
            try
            {
                var obj = await _EntityType.PostEntityType(entityType);
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
        [Route("GetCountryEntityTypeMapping")]
        public async Task<ActionResult> GetCountryEntityTypeMapping()
        {
            try
            {
                var obj = await _EntityType.GetCountryEntityTypeMapping();
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
        [Route("PostCountryEntityTypeMapping")]
        public async Task<ActionResult> PostCountryEntityTypeMapping([FromBody] EntityTypeModel entityType)
        {
            try
            {
                var obj = await _EntityType.PostCountryEntityTypeMapping(entityType);
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
        [Route("GetEntityTypeApprovalList/{UserUID}")]
        public async Task<ActionResult> GetEntityTypeApprovalList(Guid UserUID)
        {
            try
            {
                var obj = await _EntityType.GetEntityTypeApprovalList(UserUID);
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
        [Route("GetAllEntityTypeApprovalList")]
        public async Task<ActionResult> GetAllEntityTypeApprovalList()
        {
            try
            {
                var obj = await _EntityType.GetAllEntityTypeApprovalList();
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
        [Route("GetAllCountryEntityTypeMappingApproval")]
        public async Task<ActionResult> GetAllCountryEntityTypeMappingApproval()
        {
            try
            {
                var obj = await _EntityType.GetAllCountryEntityTypeMappingApproval();
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
        [Route("GetCountryEntityTypeMappingApproval/{UserUID}")]
        public async Task<ActionResult> GetCountryEntityTypeMappingApproval(Guid UserUID)
        {
            try
            {
                var obj = await _EntityType.GetCountryEntityTypeMappingApproval(UserUID);
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
        [Route("PostEntityTypeApprove")]
        public async Task<ActionResult> PostEntityTypeApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _EntityType.PostEntityTypeApprove(access);
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
        [Route("PostEntityTypeForward")]
        public async Task<ActionResult> PostEntityTypeForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _EntityType.PostEntityTypeForward(access);
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
        [Route("PostEntityTypeReject")]
        public async Task<ActionResult> PostEntityTypeReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _EntityType.PostEntityTypeReject(access);
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
        [Route("PostCountryEntityTypeApprove")]
        public async Task<ActionResult> PostCountryEntityTypeMappingApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _EntityType.PostCountryEntityTypeMappingApprove(access);
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
        [Route("PostCountryEntityTypeReject")]
        public async Task<ActionResult> PostCountryEntityTypeMappingReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _EntityType.PostCountryEntityTypeMappingReject(access);
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
        [Route("PostCountryEntityTypeForward")]
        public async Task<ActionResult> PostCountryEntityTypeForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _EntityType.PostCountryEntityTypeMappingForward(access);
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
        [Route("GetNextEntityTypeReferenceCode")]
        public async Task<ActionResult> GetNextEntityTypeReferenceCode()
        {
            try
            {
                var code = await _EntityType.GetNextEntityTypeReferenceCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
