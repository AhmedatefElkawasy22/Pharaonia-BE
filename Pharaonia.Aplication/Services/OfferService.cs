using Pharaonia.Domain.DTOs;
using Pharaonia.Domain.Models;
using System;
using System.Text.RegularExpressions;

namespace Pharaonia.Aplication.Services
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlHelperService _urlHelperService;
        private readonly IEmailSender _emailSender;

        public OfferService(IUnitOfWork unitOfWork, IUrlHelperService urlHelperService, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _urlHelperService = urlHelperService;
            _emailSender = emailSender;
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


        // Book Offer
        public async Task<ResponseModel> AddBookOfferAsync(AddBookOfferDTO model, int offerID)
        {
            try
            {
                var bookOffer = new BookOffer
                {
                    Name = model.Name,
                    ArrivalDate = model.ArrivalDate,
                    CreatedTime = DateTime.Now,
                    DepartureDate = model.DepartureDate,
                    Email = model.Email,
                    Nationality = model.Nationality,
                    NumberOfAllPeople = model.NumberOfAllPeople,
                    NumberOfChildren = model.NumberOfChildren,
                    OfferId = offerID,
                    PhoneNumber = model.PhoneNumber,
                    Requirements = model.Requirements
                };
                await _unitOfWork.BookOffer.AddAsync(bookOffer);
                await _unitOfWork.SaveChangesAsync();
                try
                {
                    var emailSubject = "New Book Offer Notification";
                    var emailBody = GenerateBookOfferEmailBody(bookOffer);
                    //send email to admin
                    await _emailSender.SendEmailAsync("elkawasyahmed@gmail.com", emailSubject, emailBody);
                    //send email to user 
                    await _emailSender.SendEmailAsync( bookOffer.Email,"Booking Confirmation - Pharaonia", GenerateBookingConfirmationEmailBody(bookOffer));
                }
                catch (Exception ex)
                {
                    return new ResponseModel
                    {
                        StatusCode = 500,
                        Message = $"An error occurred while sending the email: {ex.Message}"
                    };
                }
                return new ResponseModel
                {
                    Message = "Book offer has been Added successfully.",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = $"An error occurred while Added Book Offer : {ex.Message}"
                };
            }
        }
        public async Task<List<GetBookOfferDTO>?> GetAllBookingsAsync()
        {
            var includes = new List<Expression<Func<BookOffer, object>>> { b => b.Offer };
            var data = await _unitOfWork.BookOffer.GetAllAsync(includes);
            if (data == null || !data.Any())
                return null;

            return data.Select(b => new GetBookOfferDTO
            {
                Id = b.Id,
                Name = b.Name,
                Email = b.Email,
                Nationality = b.Nationality,
                PhoneNumber = b.PhoneNumber,
                ArrivalDate = b.ArrivalDate,
                DepartureDate = b.DepartureDate,
                NumberOfAllPeople = b.NumberOfAllPeople,
                NumberOfChildren = b.NumberOfChildren,
                Requirements = b.Requirements,
                CreatedTime = b.CreatedTime,
                Offer = b.Offer
            }).ToList();
        }
        public async Task<List<GetBookOfferDTO>?> GetAllBookingsByOfferIdAsync(int OfferID)
        {
            Expression<Func<BookOffer, bool>> match = b => b.OfferId == OfferID;
            var includes = new List<Expression<Func<BookOffer, object>>> { b => b.Offer };
            var data = await _unitOfWork.BookOffer.GetAllAsync(includes, match);
            if (data == null || !data.Any())
                return null;

            return data.Select(b => new GetBookOfferDTO
            {
                Id = b.Id,
                Name = b.Name,
                Email = b.Email,
                Nationality = b.Nationality,
                PhoneNumber = b.PhoneNumber,
                ArrivalDate = b.ArrivalDate,
                DepartureDate = b.DepartureDate,
                NumberOfAllPeople = b.NumberOfAllPeople,
                NumberOfChildren = b.NumberOfChildren,
                Requirements = b.Requirements,
                CreatedTime = b.CreatedTime,
                Offer = b.Offer
            }).ToList();
        }
        public async Task<GetBookOfferDTO?> GetBookOfferByIDAsync(int BookOfferID)
        {
            Expression<Func<BookOffer, bool>> match = b => b.Id == BookOfferID;
            var includes = new List<Expression<Func<BookOffer, object>>> { b => b.Offer };
            var data = await _unitOfWork.BookOffer.GetOneAsync(match, includes);
            if (data == null)
                return null;

            return new GetBookOfferDTO
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                Nationality = data.Nationality,
                PhoneNumber = data.PhoneNumber,
                ArrivalDate = data.ArrivalDate,
                DepartureDate = data.DepartureDate,
                NumberOfAllPeople = data.NumberOfAllPeople,
                NumberOfChildren = data.NumberOfChildren,
                Requirements = data.Requirements,
                CreatedTime = data.CreatedTime,
                Offer = data.Offer
            };
        }
        private string GenerateBookOfferEmailBody(BookOffer bookOffer)
        {
            return $@"
        <div style='font-family: Arial, sans-serif; background-color: #f4f4f9; padding: 20px; border-radius: 8px; border: 1px solid #ddd;'>
            <div style='max-width: 600px; margin: auto; background: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);'>
                <div style='padding: 20px; text-align: center; background: #4caf50; color: #ffffff;'>
                    <h1 style='margin: 0; font-size: 24px; animation: fadeIn 1s;'>🎉 New Book Offer Received! 🎉</h1>
                </div>
                <div style='padding: 20px; line-height: 1.6;'>
                    <p style='font-size: 18px; color: #555;'><strong>Name:</strong> <span style='color: #333;'>{bookOffer.Name}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Email:</strong> <span style='color: #333;'>{bookOffer.Email}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Nationality:</strong> <span style='color: #333;'>{bookOffer.Nationality}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Phone Number:</strong> <span style='color: #333;'>{bookOffer.PhoneNumber}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Arrival Date:</strong> <span style='color: #333;'>{bookOffer.ArrivalDate:yyyy-MM-dd}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Departure Date:</strong> <span style='color: #333;'>{bookOffer.DepartureDate:yyyy-MM-dd}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Number of People:</strong> <span style='color: #333;'>{bookOffer.NumberOfAllPeople}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Number of Children:</strong> <span style='color: #333;'>{bookOffer.NumberOfChildren}</span></p>
                    <p style='font-size: 18px; color: #555;'><strong>Requirements:</strong></p>
                    <p style='background: #f9f9f9; border-left: 4px solid #4caf50; padding: 10px; font-size: 16px; color: #666;'>{bookOffer.Requirements}</p>
                    <p style='font-size: 18px; color: #555;'><strong>offer booking time:</strong> <span style='color: #333;'>{bookOffer.CreatedTime:yyyy-MM-dd HH:mm}</span></p>
                </div>
                <div style='padding: 10px; text-align: center; background: #4caf50; color: #ffffff; font-size: 14px;'>
                    <p style='margin: 0;'>Thank you for using our service!</p>
                </div>
            </div>
        </div>
        <style>
            @keyframes fadeIn {{
                0% {{ opacity: 0; }}
                100% {{ opacity: 1; }}
            }}
        </style>
    ";
        }
        private string GenerateBookingConfirmationEmailBody(BookOffer bookOffer)
        {
            return $@"
        <div style='font-family: Arial, sans-serif; background-color: #f4f4f9; padding: 20px; border-radius: 8px; border: 1px solid #ddd;'>
            <div style='max-width: 600px; margin: auto; background: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);'>
                <div style='padding: 20px; text-align: center; background: #1e90ff; color: #ffffff;'>
                    <h1 style='margin: 0; font-size: 24px; animation: fadeIn 1s;'>🎉 Thank You for Booking with Pharaonia! 🎉</h1>
                </div>
                <div style='padding: 20px; line-height: 1.6;'>
                    <p style='font-size: 18px; color: #333;'>Dear <strong>{bookOffer.Name}</strong>,</p>
                    <p style='font-size: 18px; color: #555;'>We are thrilled to confirm your booking! One of our representatives will contact you shortly to assist you further.</p>
                    <p style='font-size: 18px; color: #555;'>Here are the details of your booking:</p>
                    <ul style='background: #f9f9f9; padding: 10px; border-radius: 5px; border: 1px solid #ddd;'>
                        <li style='font-size: 16px; color: #555;'>Arrival Date: <span style='color: #333;'>{bookOffer.ArrivalDate:yyyy-MM-dd}</span></li>
                        <li style='font-size: 16px; color: #555;'>Departure Date: <span style='color: #333;'>{bookOffer.DepartureDate:yyyy-MM-dd}</span></li>
                        <li style='font-size: 16px; color: #555;'>Number of People: <span style='color: #333;'>{bookOffer.NumberOfAllPeople}</span></li>
                        <li style='font-size: 16px; color: #555;'>Number of Children: <span style='color: #333;'>{bookOffer.NumberOfChildren}</span></li>
                    </ul>
                    <p style='font-size: 18px; color: #555;'>If you have any questions or need further assistance, feel free to contact us.</p>
                    <p style='font-size: 18px; color: #333;'>Thank you for choosing <strong>Pharaonia</strong>!</p>
                </div>
                <div style='padding: 10px; text-align: center; background: #1e90ff; color: #ffffff; font-size: 14px;'>
                    <p style='margin: 0;'>We look forward to serving you!</p>
                </div>
            </div>
        </div>
        <style>
            @keyframes fadeIn {{
                0% {{ opacity: 0; }}
                100% {{ opacity: 1; }}
            }}
        </style>
    ";
        }

    }
}