using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Azure.Data.Tables;
using System.Collections.Generic;

namespace ST10150702_CLDV6212_POE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableStorageController : ControllerBase
    {
        private readonly TableService _tableService;

        // Constructor that injects the TableService
        public TableStorageController(TableService tableService)
        {
            _tableService = tableService;
        }

        // GET: api/TableStorage/GetAllEntities
        [HttpGet("GetAllEntities")]
        public async Task<IActionResult> GetAllEntities()
        {
            var entities = await _tableService.GetAllEntitiesAsync();
            return Ok(entities);
        }

        // GET: api/TableStorage/GetEntity/{partitionKey}/{rowKey}
        [HttpGet("GetEntity/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetEntity(string partitionKey, string rowKey)
        {
            var entity = await _tableService.GetEntityAsync(partitionKey, rowKey);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        // POST: api/TableStorage/AddEntity
        [HttpPost("AddEntity")]
        public async Task<IActionResult> AddEntity([FromBody] TableEntity entity)
        {
            if (entity == null)
                return BadRequest("Invalid entity");

            await _tableService.AddEntityAsync(entity);
            return Ok("Entity added successfully");
        }

        // PUT: api/TableStorage/UpdateEntity
        [HttpPut("UpdateEntity")]
        public async Task<IActionResult> UpdateEntity([FromBody] TableEntity entity)
        {
            if (entity == null)
                return BadRequest("Invalid entity");

            await _tableService.UpdateEntityAsync(entity);
            return Ok("Entity updated successfully");
        }

        // DELETE: api/TableStorage/DeleteEntity/{partitionKey}/{rowKey}
        [HttpDelete("DeleteEntity/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> DeleteEntity(string partitionKey, string rowKey)
        {
            await _tableService.DeleteEntityAsync(partitionKey, rowKey);
            return Ok("Entity deleted successfully");
        }
    }
}
