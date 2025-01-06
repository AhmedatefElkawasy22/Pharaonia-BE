
using Pharaonia.Domain.Models;

namespace Pharaonia.Domain.Interfaces
{
    public interface IDestinationService
    {
        Task<List<GetDestinationDTO>> GetAllAsync();
        Task<GetDestinationDTO?> GetByIdAsync(int destinationId);
        Task<List<GetDestinationDTO>> GetBasedOnCategoryAsync(Category category);
        Task<ResponseModel> AddDestinationAsync(AddDestinationDTO model);
        Task<ResponseModel> UpdateDestinationAsync(int destinationID, UpdateDestinationDTO model);
        Task<ResponseModel> DeleteDestinationAsync(int DestinationID);
        Task<ResponseModel> AddImagesToDestinationAsync(int destinationID, List<IFormFile> images);
        Task<List<string>> GetImagesOfDestinationAsync(int DestinationID);
        Task<ResponseModel> UpdateImagesOfDestinationAsync(int DestinationID, List<IFormFile> images);

    }
}
