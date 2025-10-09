using Dms_Api.Services;
using DmsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dms_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntitiesController : ControllerBase
    {
        private readonly IEntityService _entityService;

        public EntitiesController(IEntityService entityService)
        {
            _entityService = entityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEntities()
        {
            var entities = await _entityService.GetAllEntities();
            return Ok(entities);
        }

    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetEntityById(int id)
    //    {
    //        var entity = await _entityService.GetEntityByIdAsync(id);
    //        if (entity == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(entity);
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> AddEntity([FromBody] Entity entity)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        var createdEntity = await _entityService.AddEntityAsync(entity);
    //        return CreatedAtAction(nameof(GetEntityById), new { id = createdEntity.Id }, createdEntity);
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdateEntity(int id, [FromBody] Entity entity)
    //    {
    //        if (id != entity.Id)
    //        {
    //            return BadRequest();
    //        }

    //        await _entityService.UpdateEntityAsync(entity);
    //        return NoContent();
    //    }

    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteEntity(int id)
    //    {
    //        await _entityService.DeleteEntityAsync(id);
    //        return NoContent();
    //    }
    }

}
