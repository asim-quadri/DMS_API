using ComplianceAPI.Helpers;
using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryServices _countryServices;
        private readonly CountriesFromJson _countriesFromJson;

        public CountryController(ICountryServices countryServices, CountriesFromJson countriesFromJson)
        {
            _countryServices = countryServices;
            _countriesFromJson = countriesFromJson;
        }

        [HttpGet]
        [Route("GetAllCountryMaster")]
        public async Task<ActionResult> GetAllCountryMaster(int userId)
        {
            try
            {
                var obj = await _countryServices.GetAllCountryMaster(userId);
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
        [Route("GetAllCountriesFromJsonFile")]
        public ActionResult GetAllCountriesFromJsonFile()
        {
            try
            {
                var obj = _countriesFromJson.GetCountries();
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
        [Route("GetCountryByOrgId/{id}")]
        public async Task<ActionResult> GetCountryByOrgId(int id)
        {
            try
            {
                var obj = await _countryServices.GetCountryByOrgId(id);
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

                //country.UID = Guid.Parse("16193229-D8DC-4B89-8209-4E2FADE34930");

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
        [Route("GetStateById/{CountryId}/{userId}")]
        public async Task<ActionResult> GetStateById(int CountryId, int userId)
        {
            try
            {
                var obj = await _countryServices.GetStateById(CountryId, userId);
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
        public async Task<ActionResult> PostCountryStateMapping([FromBody] CountryStateMappingModel country)
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
        public async Task<ActionResult> DeleteCountryStateMapping([FromBody] CountryStateMappingModel country)
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
        [Route("GetAllCountryStateApproval")]
        public async Task<ActionResult> GetAllCountryStateApproval()
        {
            try
            {
                var obj = await _countryServices.GetAllCountryStateApproval();
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

        [HttpGet]
        [Route("GetAllCountryStateMappingApproval")]
        public async Task<ActionResult> GetAllCountryStateMappingApproval()
        {
            try
            {
                var obj = await _countryServices.GetAllCountryStateMappingApprovaList();
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

        [HttpGet]
        [Route("GetAllCurrencyCodes")]
        public async Task<ActionResult> GetAllCurrencyCodes()
        {
            try
            {
                var obj = await _countryServices.GetAllCurrencyCodes();
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostUserCountryMapping")]
        public async Task<ActionResult> PostUserCountryMapping([FromBody] List<UserCountryMappingModel> userCountryMappings)
        {
            try
            {
                var obj = await _countryServices.PostUserCountryMapping(userCountryMappings);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostUserStateMapping")]
        public async Task<ActionResult> PostUserStateMapping([FromBody] List<UserStateMappingModel> userStates)
        {
            try
            {
                if (userStates == null || !userStates.Any())
                {
                    return BadRequest("User states cannot be null or empty.");
                }
                // Assuming there's a method in _accessServices to save user states
                var result = await _countryServices.PostUserStateMapping(userStates);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserStateMapping")]
        public async Task<ActionResult> GetUserStateMapping(int userId)
        {
            try
            {
                var obj = await _countryServices.GetUserStateMapping(userId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetServiceReqAndBillingDetails/{userId}/{countryId}")]
        public async Task<ActionResult> GetServiceReqAndBillingDetails(long userId, long countryId)
        {
            try
            {
                var obj = await _countryServices.GetServiceReqAndBillingDetails(userId, countryId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetServiceReqAndBillingDetailsByState/{userId}/{stateId}")]
        public async Task<ActionResult> GetServiceReqAndBillingDetailsByState(long userId, long stateId)
        {
            try
            {
                var obj = await _countryServices.GetServiceReqAndBillingDetailsByState(userId, stateId);
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
        [Route("GetNextStateCode")]
        public async Task<ActionResult> GetNextStateCode()
        {
            try
            {
                var code = await _countryServices.GetNextStateCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetNextCountryCode")]
        public async Task<ActionResult> GetNextCountryCode()
        {
            try
            {
                var code = await _countryServices.GetNextCountryCode();
                return Ok(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetLastCountryIndex")]
        public async Task<ActionResult> GetLastCountryReferenceCode()
        {
            try
            {
                var result = await _countryServices.GetLastCountryReferenceCode();
                if (!string.IsNullOrEmpty(result))
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetLastStateIndex")]
        public async Task<ActionResult> GetLastStateReferenceCode()
        {
            try
            {
                var result = await _countryServices.GetLastStateReferenceCode();
                if (!string.IsNullOrEmpty(result))
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}