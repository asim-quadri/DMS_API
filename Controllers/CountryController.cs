using DmsApi.Models;
using DmsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dms_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryServices _countryServices;
        public CountryController(ICountryServices countryServices)
        {
            _countryServices = countryServices;
        }

        [HttpGet]
        [Route("GetAllCountries")]
        public async Task<ActionResult> GetAllCountries()
        {
            try
            {
                var obj = await _countryServices.GetAllCountries();
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
        [Route("GetCountryByUID/{uid}")]
        public async Task<ActionResult> GetCountryByUID(Guid uid)
        {
            try
            {
                var obj = await _countryServices.GetCountryByUID(uid);
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
        [Route("GetStates/{CountryCode}")]
        public async Task<ActionResult> GetStatesByCountry(int CountryCode)
        {
            try
            {
                var obj = await _countryServices.GetStatesByCountry(CountryCode);
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
        [Route("PostCountry")]
        public async Task<ActionResult> PostCountry([FromBody] Country country)
        {
            try
            {
                var obj = await _countryServices.PostCountry(country);
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
        [Route("PostState")]
        public async Task<ActionResult> PostState([FromBody] States states)
        {
            try
            {
                var obj = await _countryServices.PostState(states);
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
        [Route("GetStateById/{CountryId}")]
        public async Task<ActionResult> GetStateById(int CountryId)
        {
            try
            {
                var obj = await _countryServices.GetStateById(CountryId);
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
        [Route("GetCountryStateMapping")]
        public async Task<ActionResult> GetCountryStateMapping()
        {
            try
            {
                var obj = await _countryServices.GetCountryStatesMapping();
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
        [Route("PostCountryStateMapping")]
        public async Task<ActionResult> PostCountryStateMapping([FromBody] CountryStateMapping country)
        {
            try
            {
                var obj = await _countryServices.PostCountryStateMapping(country);
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
        [Route("DeleteCountryStateMapping")]
        public async Task<ActionResult> DeleteCountryStateMapping([FromBody] CountryStateMapping country)
        {
            try
            {
                var obj = await _countryServices.DeleteCountryStateMapping(country);
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
        [Route("GetCountryStateApproval/{UserUID}")]
        public async Task<ActionResult> GetCountryStateApproval(Guid UserUID)
        {
            try
            {
                var obj = await _countryServices.GetCountryApprovaList(UserUID);
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
        [Route("GetCountryStateMappingApproval/{UserUID}")]
        public async Task<ActionResult> GetCountryStateMappingApproval(Guid UserUID)
        {
            try
            {
                var obj = await _countryServices.GetCountryStateMappingApprovaList(UserUID);
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
        [Route("PostCountryApprove")]
        public async Task<ActionResult> PostCountryApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostCountryApprove(access);
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
        [Route("PostCountryReject")]
        public async Task<ActionResult> PostCountryReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostCountryReject(access);
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
        [Route("PostCountryForward")]
        public async Task<ActionResult> PostCountryForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostCountryForward(access);
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
        [Route("PostStateApprove")]
        public async Task<ActionResult> PostStateApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostStateApprove(access);
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
        [Route("PostStateReject")]
        public async Task<ActionResult> PostStateReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostStateReject(access);
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
        [Route("PostStateForward")]
        public async Task<ActionResult> PostStateForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostStateForward(access);
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
        [Route("PostCountryStateApprove")]
        public async Task<ActionResult> PostCountryStateMappingApprove([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostCountryStateMappingApprove(access);
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
        [Route("PostCountryStateReject")]
        public async Task<ActionResult> PostCountryStateMappingReject([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostCountryStateMappingReject(access);
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
        [Route("PostCountryStateForward")]
        public async Task<ActionResult> PostCountryStateMappingForward([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _countryServices.PostCountryStateMappingForward(access);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
