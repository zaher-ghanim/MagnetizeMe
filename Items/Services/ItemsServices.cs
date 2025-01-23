using ItemsAPI.Interface;
using Microsoft.EntityFrameworkCore;
using RepositoryAPI;
using RepositoryAPI.Models;

namespace ItemsAPI.Services
{
    public class ItemsServices : IItemsServices
    {
        private readonly EcommerceDbContext _context;

        public ItemsServices(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var newItem = await _context.Items.AddAsync(item);
            _context.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return await Task.FromResult(false);
            }
            _context.Items.Remove(item);
            _context.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemById(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return null;
            }
            return item;
        }

        public async Task<List<Item>> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            return items;
        }

        public async Task<bool> UpdateItem(int id, Item item)
        {
            var dbitem = await _context.Items.FindAsync(id);
            if (dbitem == null)
            {
                return await Task.FromResult(false);
            }
            dbitem.ItemDesc=item.ItemDesc;

            _context.SaveChanges();
            return await Task.FromResult(true);
        }
    }
}
