using LearnCosmosDB.Models;
using LearnCosmosDB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnCosmosDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildController(IChildService childDbService)
        {
            _childService = childDbService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Child item)
        {
            var response = await _childService.CreateChildAsync(item);
            return Ok(response.Resource);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChild([FromBody] Child item)
        {
            var response = await _childService.UpdateChildAsync(item);
            return Ok(response.Resource);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var item = await _childService.GetChildAsync(id, id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _childService.GetChildrenAsync();
            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await _childService.DeleteChildAsync(id, id);
            return Ok("Deletion completed");
        }
    }
}
