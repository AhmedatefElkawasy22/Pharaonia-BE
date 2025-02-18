
using Pharaonia.Domain.Models;

namespace Pharaonia.Domain.Interfaces
{
    public interface IDestinationService
    {
        Task<List<GetDestinationDTO>> GetAllAsync();
        Task<GetDestinationDTO?> GetByIdAsync(int destinationId);
        Task<List<GetDestinationDTO>> GetBasedOnNumberAsync(int number);
        Task<List<GetDestinationDTO>> GetBasedOnCategoryAsync(Category category);
        Task<ResponseModel> AddDestinationAsync(AddDestinationDTO model);
        Task<ResponseModel> UpdateDestinationAsync(int destinationID, UpdateDestinationDTO model);
        Task<ResponseModel> DeleteDestinationAsync(int DestinationID);
        Task<ResponseModel> AddImagesToDestinationAsync(int destinationID, List<IFormFile> images);
        Task<List<GetImageDTO>> GetImagesOfDestinationAsync(int DestinationID);
        Task<ResponseModel> UpdateImagesOfDestinationAsync(int DestinationID, List<IFormFile> images);
        Task<ResponseModel> DeleteImageFromDestinationAsync(int ImageID);
        Task<ResponseModel> DeleteAllImageFromDestinationAsync(int DestinationID);

    }
}
