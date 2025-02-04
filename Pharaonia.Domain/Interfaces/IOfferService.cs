
namespace Pharaonia.Domain.Interfaces
{
    public interface IOfferService
    {
        Task<List<GetOfferDTO>> GetAllOffersAsync();
        Task<GetOfferDTO?> GetOfferByIdAsync(int OfferID);
        Task<ResponseModel> DeleteOfferAsync(int offerId);
        Task<ResponseModel> AddOfferAsync(AddOfferDTO model);
        Task<ResponseModel> UpdateOfferAsync(int offerId, UpdateOfferDTO model);
        Task<List<GetOfferDTO>> GetAvailableOffersAsync();
        Task<List<GetOfferDTO>> GetOffersExpiredAsync();
        Task<ResponseModel> ReactivateOfferAsync(int OfferID);
        Task<List<string>> GetImagesOfOfferAsync(int OfferID);
        Task<ResponseModel> AddImagesToOfferAsync(int OfferID, List<IFormFile> images);
        Task<ResponseModel> UpdateImagesOfOfferAsync(int OfferID, List<IFormFile> images);
        Task<ResponseModel> DeleteImageFromOfferAsync(int ImageID);
        Task<ResponseModel> DeleteAllImageFromOfferAsync(int OfferID);
        //book offers
        Task<ResponseModel> AddBookOfferAsync(AddBookOfferDTO model, int offerID);
        Task<List<GetBookOfferDTO>?> GetAllBookingsAsync();
        Task<List<GetBookOfferDTO>?> GetAllBookingsByOfferIdAsync(int OfferID);
        Task<GetBookOfferDTO?> GetBookOfferByIDAsync(int BookOfferID);

    }
}
