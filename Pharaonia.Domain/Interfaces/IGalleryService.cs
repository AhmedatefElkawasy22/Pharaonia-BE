
namespace Pharaonia.Domain.Interfaces
{
    public interface IGalleryService
    {
        Task<IEnumerable<Gallery>> GetAllAsync();
        Task<Gallery?> GetByIdAsync(int id);
        Task<ResponseModel> DeleteAsync(int id);
        Task<ResponseModel> DeleteAllAsync();
        Task<ResponseModel> AddAsync(List<IFormFile> images);
    }
}
