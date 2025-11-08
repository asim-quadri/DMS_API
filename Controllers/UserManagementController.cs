using ComplianceAPI.Models;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserServices _usersServices;

        public UserManagementController(IUserServices userServices)
        {
            _usersServices = userServices;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            try
            {
                var obj = await _usersServices.login(login);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var obj = await _usersServices.GetAllUsers();
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
        [Route("GetUsers/{uid}")]
        public async Task<ActionResult> GetUsers(Guid uid)
        {
            var obj = await _usersServices.GetUsers(uid);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetHistoryUsers/{HistoryId}")]
        public async Task<ActionResult> GetHistoryUsers(long HistoryId)
        {
            var obj = await _usersServices.GetHistoryUsers(HistoryId);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetHistoryRoles/{HistoryId}")]
        public async Task<ActionResult> GetHistoryRoles(long HistoryId)
        {
            var obj = await _usersServices.GetHistoryRoles(HistoryId);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpPost]
        [Route("PostUser")]
        public async Task<ActionResult> PostUser([FromBody] PostUser user, Guid? accessUID)
        {
            try
            {
                var obj = await _usersServices.PostUser(user, accessUID);
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
        [Route("UpdateRole")]
        public async Task<ActionResult> UpdateRole([FromBody] UpdateRoles roles)
        {
            try
            {
                var obj = await _usersServices.Updateroles(roles);
                if (obj != null)
                    return Ok(obj);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("AddRoles")]
        //public JsonResult AddRole(Roles role)
        //{
        //    var obj = new UsersRepository().AddRole(role);
        //    if (obj == null)
        //    {
        [HttpDelete]
        [Route("deleteUsers/{uid}/{status}")]
        public async Task<ActionResult> deleteUsers(Guid uid, int status)
        {
            var obj = await _usersServices.DeleteUsers(uid, status);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<ActionResult> GetAllRoles()
        {
            var obj = await _usersServices.GetAllRoles();
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetRoles/{uid}")]
        public async Task<ActionResult> GetRoles(Guid uid)
        {
            var obj = await _usersServices.GetRoles(uid);
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            var obj = await _usersServices.GetAllProducts();
            if (obj != null)
                return Ok(obj);
            return NotFound();
        }

        //[HttpPost]
        //[Route("PostProductAccess")]
        //public async Task<ActionResult> PostProductAccess([FromBody] Products products)
        //{
        //    try
        //    {
        //        var obj = await _usersServices.PostProductAccess(products);
        //        if (obj != null)
        //            return Ok(obj);
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            try
            {
                var obj = await _usersServices.ForgotPassword(forgotPassword);
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
        [Route("GetAllUsersApproval/{UserUID}")]
        public async Task<ActionResult> GetAllUsersApproval(Guid? UserUID)
        {
            try
            {
                var obj = await _usersServices.GetPendingUserApproval(UserUID);
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
        [Route("GetAllRolesApproval/{UserUID}")]
        public async Task<ActionResult> GetAllRolesApproval(Guid? UserUID)
        {
            try
            {
                var obj = await _usersServices.GetPendingRoleApproval(UserUID);
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
        [Route("UpdateApproveAccess")]
        public async Task<ActionResult> UpdateApproveAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _usersServices.UpdateRoleApproveAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateRejectAccess")]
        public async Task<ActionResult> UpdateRejectAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _usersServices.UpdateRoleRejectAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostApproveAccess")]
        public async Task<ActionResult> PostApproveAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _usersServices.PostUserApproveAccess(access);
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
                var obj = await _usersServices.PostUserRejectAccess(access);
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
                var obj = await _usersServices.PostUserReviewAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostForwardAccess")]
        public async Task<ActionResult> PostForwardAccess([FromBody] AccessModel access)
        {
            try
            {
                var obj = await _usersServices.PostUserForwardAccess(access);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}