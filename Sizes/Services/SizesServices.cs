using Microsoft.EntityFrameworkCore;
using RepositoryAPI;
using RepositoryAPI.Models;
using SizesAPI.DTO;
using SizesAPI.Interface;

namespace SizesAPI.Services
{
    public class SizesServices : ISizesServices
    {

        private readonly EcommerceDbContext _context;

        public SizesServices(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddSize(SizeDTO size)
        {
            if (size == null) throw new ArgumentNullException(nameof(size));

            // Map SizeDTO to Size entity
            var newSize = new Size
            {
                ItemId = size.ItemId,
                SizeDesc = size.SizeDesc,
                Dimension = size.Dimension,
                Quantity = size.Quantity,
                StepQuantity = size.StepQuantity,
                Price = size.Price
            };

            // Add the new Size entity to the context
            await _context.Sizes.AddAsync(newSize);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return await Task.FromResult(false);
            }
            _context.Sizes.Remove(size);
            _context.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<Size> GetSizeById(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return null;
            }
            return size;
        }

        public async Task<List<Size>> GetSizes()
        {
            var sizes = await _context.Sizes.ToListAsync();
            return sizes;
        }

        public async Task<bool> UpdateSize(int id, Size size)
        {
var dbSize=await _context.Sizes.FindAsync(id);
            if (dbSize == null) return await Task.FromResult(false);

            dbSize.ItemId = size.ItemId;
            dbSize.SizeDesc = size.SizeDesc;
            dbSize.Dimension = size.Dimension;
            dbSize.Quantity = size.Quantity;
            dbSize.StepQuantity = size.StepQuantity;
            dbSize.Price = size.Price;
            _context.SaveChanges();
            return await Task.FromResult(true);
            
        }
    }
}
