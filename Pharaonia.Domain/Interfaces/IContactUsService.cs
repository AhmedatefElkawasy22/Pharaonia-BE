
namespace Pharaonia.Domain.Interfaces
{
    public interface IContactUsService
    {
        Task<ResponseModel> AddAsync(AddContactUSDTO model);
        Task<ResponseModel> DeleteAsync(int id);
        Task<ContactUS?> GetByIdAsync(int id);
        Task<List<ContactUS>> GetAllAsync();
        Task<ResponseModel> MarkAsContacted(int id);
    }
}
