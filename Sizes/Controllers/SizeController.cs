using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryAPI.Models;
using SizesAPI.DTO;
using SizesAPI.Interface;

namespace SizesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]


    public class SizeController : ControllerBase
    {
        private readonly ISizesServices _sizeService;
        public SizeController(ISizesServices sizesServices)
        {
            _sizeService = sizesServices;
        }

        // GET: api/sizes
        [HttpGet]
        public async Task<IEnumerable<Size>> GetAll()
        {
            List<Size> sizes = await this._sizeService.GetSizes();
            return sizes;
        }

        // GET: api/sizes/{id}
        [HttpGet("{id}")]
        public async Task<Size> GetSize(int id)
        {
            Size retVal = await _sizeService.GetSizeById(id);
            return retVal;
        }

        // POST: api/sizes
        [HttpPost]
        public async Task<bool> AddNewSize([FromBody] SizeDTO size)
        {
            return await _sizeService.AddSize(size);
        }

        // PUT: api/sizes/{id}
        [HttpPut("{id}")]
        public async Task<bool> UpdateSize(int id, [FromBody] Size size)
        {
            return await _sizeService.UpdateSize(id, size);
        }

        // DELETE: api/sizes/{id}
        [HttpDelete("{id}")]
        public async Task<bool> DeleteSize(int id)
        {
            return await _sizeService.DeleteSize(id);
        }
    }
}
