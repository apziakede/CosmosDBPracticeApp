using LearnCosmosDB.Models;
using LearnCosmosDB.Repositories;
using LearnCosmosDB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnCosmosDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyService _familyService;

        public FamilyController(IFamilyService familyService)
        {
            _familyService = familyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Family item)
        {
            var response = await _familyService.CreateFamilyAsync(item);
            return Ok(response.Resource);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFamily([FromBody] Family item)
        {
            var response = await _familyService.UpdateFamilyAsync(item);
            return Ok(response.Resource);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var item = await _familyService.GetFamilyAsync(id, id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _familyService.GetFamiliesAsync();
            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await _familyService.DeleteFamilyAsync(id, id);
            return Ok("Deletion completed");
        }
    }
}
