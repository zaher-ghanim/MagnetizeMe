using RepositoryAPI.Models;
using SizesAPI.DTO;

namespace SizesAPI.Interface
{
    public interface ISizesServices
    {
        Task<List<Size>> GetSizes();
        Task<Size> GetSizeById(int id);
        public Task<bool> AddSize(SizeDTO size);
        Task<bool> UpdateSize(int id, Size size);
        Task<bool> DeleteSize(int id);
    }
}
