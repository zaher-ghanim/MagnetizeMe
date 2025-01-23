using RepositoryAPI.Models;

namespace ItemsAPI.Interface
{
    public interface IItemsServices
    {
        Task<List<Item>> GetItems();
        Task<Item> GetItemById(int id);
        public Task<bool> AddItem(Item item);
        Task<bool> UpdateItem(int id, Item item);
        Task<bool> DeleteItem(int id);
    }
}
