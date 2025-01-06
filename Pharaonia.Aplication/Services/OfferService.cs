using Pharaonia.Domain.DTOs;
using Pharaonia.Domain.Models;
using System;

namespace Pharaonia.Aplication.Services
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlHelperService _urlHelperService;

        public OfferService(IUnitOfWork unitOfWork, IUrlHelperService urlHelperService)
        {
            _unitOfWork = unitOfWork;
            _urlHelperService = urlHelperService;
        }

        public async Task<List<GetOfferDTO>> GetAllOffersAsync()
        {
            var includes = new List<Expression<Func<Offer, object>>> { o => o.Images };
            var offers = await _unitOfWork.Offers.GetAllAsync(includes);

            return offers.Select(e => new GetOfferDTO
            {
                Id = e.Id,
                Description = e.Description,
                Price = e.Price,
                ExpireOn = e.ExpireOn,
                NameOfDestination = e.NameOfDestination,
                OfferDuration = $"{e.OfferDurationNumber} {(e.TypeOfOfferDuration == TypeOfTime.day ? "Days" : "Hours")}",
                Images = e.Images.Select(e => e.ImagePath).ToList()
            }).ToList();
        }

        public async Task<GetOfferDTO?> GetOfferByIdAsync(int offerId)
        {
            var includes = new List<Expression<Func<Offer, object>>> { o => o.Images };
            Expression<Func<Offer, bool>> match = o => o.Id == offerId;
            var offer = await _unitOfWork.Offers.GetOneAsync(match, includes);

            if (offer == null)
                return null;

            return new GetOfferDTO
            {
                Id = offer.Id,
                Description = offer.Description,
                Price = offer.Price,
                ExpireOn = offer.ExpireOn,
                NameOfDestination = offer.NameOfDestination,
                OfferDuration = $"{offer.OfferDurationNumber} {(offer.TypeOfOfferDuration == TypeOfTime.day ? "Days" : "Hours")}",
                Images = offer.Images.Select(e => e.ImagePath).ToList()
            };
        }

        public async Task<ResponseModel> AddOfferAsync(AddOfferDTO model)
        {
            try
            {
                Offer newOffer = new()
                {
                    NameOfDestination = model.NameOfDestination,
                    Description = model.Description,
                    Price = model.Price,
                    OfferDurationNumber = model.OfferDurationNumber,
                    TypeOfOfferDuration = model.TypeOfOfferDuration,
                    OfferExpirationNumber = model.OfferExpirationNumber,
                    TypeOfOfferExpirationDate = model.TypeOfOfferExpirationDate,
                    ExpireOn = model.TypeOfOfferExpirationDate == TypeOfTime.day ?
                                                   DateTime.Now.AddDays(model.OfferExpirationNumber)
                                                  : DateTime.Now.AddHours(model.OfferExpirationNumber)
                };
                await _unitOfWork.Offers.AddAsync(newOffer);
                await _unitOfWork.SaveChangesAsync();

                if (model.Images == null || !model.Images.Any())
                    return new ResponseModel { Message = "offer has been added successfully without any image", StatusCode = 200 };

                var res = await AddImageAsync(newOffer, model.Images);
                if (res.StatusCode == 200)
                    res.Message = "offer has been added successfully with all images.";

                return res;
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while adding Offer: {ex.Message}" };
            }

        }

        public async Task<ResponseModel> UpdateOfferAsync(int offerId, UpdateOfferDTO model)
        {
            try
            {
                Expression<Func<Offer, bool>> match = e => e.Id == offerId;
                Offer? offer = await _unitOfWork.Offers.GetOneAsync(match);
                if (offer == null) return new ResponseModel { Message = "Offer Not Exist.", StatusCode = 400 };

                offer.NameOfDestination = model.NameOfDestination;
                offer.Price = model.Price;
                offer.Description = model.Description;
                offer.OfferDurationNumber = model.OfferDurationNumber;
                offer.TypeOfOfferDuration = model.TypeOfOfferDuration;
                offer.OfferExpirationNumber = model.OfferExpirationNumber;
                offer.TypeOfOfferExpirationDate = model.TypeOfOfferExpirationDate;
                offer.ExpireOn = model.TypeOfOfferExpirationDate == TypeOfTime.day ?
                                                   DateTime.Now.AddDays(model.OfferExpirationNumber)
                                                  : DateTime.Now.AddHours(model.OfferExpirationNumber);
                _unitOfWork.Offers.Update(offer);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel { Message = "Offer has been updated successfully.", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while update offer: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteOfferAsync(int offerId)
        {
            try
            {
                var includes = new List<Expression<Func<Offer, object>>> { o => o.Images };
                Expression<Func<Offer, bool>> match = o => o.Id == offerId;
                var offer = await _unitOfWork.Offers.GetOneAsync(match, includes);
                if (offer == null) return new ResponseModel { Message = "Offer Not Exist.", StatusCode = 400 };
                // Delete images 
                foreach (var image in offer.Images)
                {
                    string oldImagePath = Path.Combine("wwwroot", "Images_of_Offers", Path.GetFileName(image.ImagePath));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }
                _unitOfWork.OfferImages.RemoveRange(offer.Images);
                _unitOfWork.Offers.Remove(offer);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Offer has been Deleted Successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while Delete Offer: {ex.Message}" };
            }
        }

        public async Task<List<GetOfferDTO>> GetAvailableOffersAsync()
        {
            Expression<Func<Offer, bool>> match = o => o.ExpireOn > DateTime.Now;
            var includes = new List<Expression<Func<Offer, object>>>() { i => i.Images };
            var offers = await _unitOfWork.Offers.GetAllAsync(includes, match);
            if (offers is null)
                return new List<GetOfferDTO>();

            return offers.Select(o => new GetOfferDTO
            {
                Id = o.Id,
                Description = o.Description,
                Price = o.Price,
                ExpireOn = o.ExpireOn,
                NameOfDestination = o.NameOfDestination,
                OfferDuration = $"{o.OfferDurationNumber} {(o.TypeOfOfferDuration == TypeOfTime.day ? "Days" : "Hours")}",
                Images = o.Images.Select(img => img.ImagePath).ToList()
            })
           .ToList();
        }

        public async Task<List<string>> GetImagesOfOfferAsync(int OfferID)
        {
            Expression<Func<Offer, bool>> match = o => o.Id == OfferID;
            var includes = new List<Expression<Func<Offer, object>>>() { i => i.Images };
            var offer = await _unitOfWork.Offers.GetOneAsync(match, includes);
            if (offer is null)
                return new List<string>();

            return offer.Images.Select(img => img.ImagePath).ToList();
        }

        public async Task<List<GetOfferDTO>> GetOffersExpiredAsync()
        {
            Expression<Func<Offer, bool>> match = o => o.ExpireOn < DateTime.Now;
            var includes = new List<Expression<Func<Offer, object>>>() { i => i.Images };
            var offers = await _unitOfWork.Offers.GetAllAsync(includes, match);
            if (offers is null)
                return new List<GetOfferDTO>();

            return offers.Select(o => new GetOfferDTO
            {
                Id = o.Id,
                Description = o.Description,
                Price = o.Price,
                ExpireOn = o.ExpireOn,
                NameOfDestination = o.NameOfDestination,
                OfferDuration = $"{o.OfferDurationNumber} {(o.TypeOfOfferDuration == TypeOfTime.day ? "Days" : "Hours")}",
                Images = o.Images.Select(img => img.ImagePath).ToList()
            })
          .ToList();
        }

        public async Task<ResponseModel> AddImagesToOfferAsync(int offerId, List<IFormFile> images)
        {
            if (images == null || !images.Any())
                return new ResponseModel { Message = "No images provided to add.", StatusCode = 400 };

            Expression<Func<Offer, bool>> match = o => o.Id == offerId;
            var includes = new List<Expression<Func<Offer, object>>>() { i => i.Images };
            var offer = await _unitOfWork.Offers.GetOneAsync(match, includes);
            if (offer == null)
                return new ResponseModel { Message = "Offer does not exist.", StatusCode = 400 };

            var response = await AddImageAsync(offer, images);

            return response;
        }

        public async Task<ResponseModel> ReactivateOfferAsync(int offerId)
        {
            try
            {
                Expression<Func<Offer, bool>> match = o => o.Id == offerId;
                var includes = new List<Expression<Func<Offer, object>>>() { i => i.Images };
                var offer = await _unitOfWork.Offers.GetOneAsync(match, includes);
                if (offer == null) return new ResponseModel { Message = "Offer does not exist.", StatusCode = 400 };
                offer.ExpireOn = offer.TypeOfOfferExpirationDate == TypeOfTime.day ? DateTime.Now.AddDays(offer.OfferExpirationNumber)
                                                                    : DateTime.Now.AddHours(offer.OfferExpirationNumber);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { Message = "Offer is Available from now.", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while Reactivate Offer: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> UpdateImagesOfOfferAsync(int OfferID, List<IFormFile> images)
        {
            try
            {
                if (images == null || !images.Any())
                    return new ResponseModel { Message = "No images provided for update.", StatusCode = 400 };

                Expression<Func<Offer, bool>> match = o => o.Id == OfferID;
                var includes = new List<Expression<Func<Offer, object>>>() { i => i.Images };
                var offer = await _unitOfWork.Offers.GetOneAsync(match, includes);
                if (offer == null) return new ResponseModel { Message = "Offer Not Exist.", StatusCode = 400 };

                // Delete old images 
                foreach (var image in offer.Images)
                {
                    string oldImagePath = Path.Combine("wwwroot", "Images_of_Offers", Path.GetFileName(image.ImagePath));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }
                _unitOfWork.OfferImages.RemoveRange(offer.Images);
                await _unitOfWork.SaveChangesAsync();
                // add new images
                var res = await AddImageAsync(offer, images);
                if (res.StatusCode == 200)
                    res.Message = "All images has been updated successfully.";
                return res;
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while update images: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteImageFromOfferAsync(int ImageID)
        {
            try
            {
                Expression<Func<OfferImages, bool>> match = i => i.Id == ImageID;
                var image = await _unitOfWork.OfferImages.GetOneAsync(match);
                if (image == null)
                    return new ResponseModel { Message = "This image does not exist.", StatusCode = 400 };

                string oldImagePath = Path.Combine("wwwroot", "Images_of_Offers", Path.GetFileName(image.ImagePath));
                if (File.Exists(oldImagePath))
                    File.Delete(oldImagePath);

                _unitOfWork.OfferImages.Remove(image);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel
                {
                    Message = "Image has been deleted successfully.",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = $"An error occurred while deleting the image: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> DeleteAllImageFromOfferAsync(int OfferID)
        {
            try
            {
                Expression<Func<Offer, bool>> match = o => o.Id == OfferID;
                var includes = new List<Expression<Func<Offer, object>>>() { i => i.Images };
                var offer = await _unitOfWork.Offers.GetOneAsync(match, includes);
                if (offer == null)
                    return new ResponseModel { Message = "This offer does not exist.", StatusCode = 400 };
                foreach (var image in offer.Images)
                {
                    string oldImagePath = Path.Combine("wwwroot", "Images_of_Offers", Path.GetFileName(image.ImagePath));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }
                _unitOfWork.OfferImages.RemoveRange(offer.Images);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel
                {
                    Message = "Images has been deleted successfully.",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = $"An error occurred while deleting the images: {ex.Message}"
                };
            }
        }

        private async Task<ResponseModel> AddImageAsync(Offer offer, List<IFormFile> images)
        {
            try
            {
                string imageDirectory = Path.Combine("wwwroot", "Images_of_Offers");

                if (!Directory.Exists(imageDirectory))
                    Directory.CreateDirectory(imageDirectory);

                List<string> invalidFiles = new();

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };

                foreach (var formFile in images)
                {
                    var extension = Path.GetExtension(formFile.FileName)?.ToLowerInvariant();
                    if (formFile.Length <= 5 * 1024 * 1024 && allowedExtensions.Contains(extension))
                    {
                        string fileName = $"{offer.NameOfDestination}_{Guid.NewGuid()}.jpg";
                        string filePath = Path.Combine(imageDirectory, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        string imageUrl = $"{_urlHelperService.GetCurrentServerUrl()}/Images_of_Offers/{fileName}";

                        await _unitOfWork.OfferImages.AddAsync(new OfferImages
                        {
                            ImagePath = imageUrl,
                            OfferId = offer.Id,
                        });
                    }
                    else
                    {
                        invalidFiles.Add(formFile.FileName);
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                if (invalidFiles.Any())
                    return new ResponseModel { StatusCode = 206, Message = $"some files were not uploaded because they exceeded the size limit of 5 MB or were not in the allowed formats (PNG,JPG,jpeg). Invalid files: {string.Join(", ", invalidFiles)}" };

                return new ResponseModel { StatusCode = 200, Message = "All images have been successfully added." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while adding images: {ex.Message}" };
            }

        }

    }
}