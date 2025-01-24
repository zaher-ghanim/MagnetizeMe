using ItemsAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryAPI.Models;

namespace ItemsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]

    public class ItemsController : ControllerBase
    {
        private readonly IItemsServices _itemsServices;
        public ItemsController(IItemsServices itemsServices)
        {
            _itemsServices = itemsServices;
        }


        // GET: api/items
        [HttpGet]

        public async Task<IEnumerable<Item>> GetAll()
        {
            List<Item> items = await this._itemsServices.GetItems();
            return items;
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<Item> GetItem(int id)
        {
            Item retVal = await _itemsServices.GetItemById(id);
            return retVal;
        }

        // POST: api/items
        [HttpPost]
        public async Task<bool> AddNewItems([FromBody] Item item)
        {
            return await _itemsServices.AddItem(item);
        }

        // PUT: api/items/{id}
        [HttpPut("{id}")]
        public async Task<bool> UpdateItem(int id, [FromBody] Item item)
        {
            return await _itemsServices.UpdateItem(id, item);
        }

        // DELETE: api/items/{id}
        [HttpDelete("{id}")]
        public async Task<bool> DeleteItem(int id)
        {
            return await _itemsServices.DeleteItem(id);
        }
    }
}
