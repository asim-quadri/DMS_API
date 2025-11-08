using ComplianceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComplianceAPI.Services;
using Microsoft.AspNetCore.Cors;
using ComplianceAPI.Repository;

namespace ComplianceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderManagement : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FolderManagement(IFolderService folderService)
        {
            _folderService = folderService;
        }
        [HttpPost]
        [Route("create-folder")]
        public async Task<ActionResult> CreateFolder([FromBody] Folder folder)
        {
            var obj = await _folderService.CreateFolder(folder);
            if (obj != null)
                return Ok(obj);
            return NotFound();
            
        }


        [HttpGet]
        [Route("FoldersByEntity")]
        public async Task<ActionResult> FoldersListByEntity(int entityId)
        {
            try
            {
                var obj = await _folderService.GetFoldersListByEntity(entityId);
                if (obj != null)
                    return Ok(obj);
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("tree")]
        public async Task<IActionResult> GetFolderTree(int intityId, int userId,string? mtype="Dms")
        {
            var folderTree = await _folderService.GetFolderTreeAsync(intityId, userId,mtype);
            return Ok(folderTree);
        }
    }
}
