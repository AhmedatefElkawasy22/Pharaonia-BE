
namespace Pharaonia.Aplication.Services
{
    public class DestinationService : IDestinationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlHelperService _urlHelperService;
        public DestinationService(IUnitOfWork unitOfWork, IUrlHelperService urlHelperService)
        {
            _unitOfWork = unitOfWork;
            _urlHelperService = urlHelperService;
        }

        public async Task<List<GetDestinationDTO>> GetAllAsync()
        {
            var includes = new List<Expression<Func<Destination, object>>> { i => i.Images };

            var destinations = await _unitOfWork.Destinations.GetAllAsync(includes);

            if (destinations == null)
                return new List<GetDestinationDTO>();

            return destinations.Select(destination => new GetDestinationDTO
            {
                Id = destination.Id,
                Description = destination.Description,
                DestinationCategory = destination.DestinationCategory.ToString(),
                Name = destination.Name,
                ImagePath = destination.Images.Select(i => i.ImagePath).ToList()
            }).ToList();
        }

        public async Task<GetDestinationDTO?> GetByIdAsync(int destinationId)
        {
            var includes = new List<Expression<Func<Destination, object>>> { o => o.Images };
            Expression<Func<Destination, bool>> match = o => o.Id == destinationId;
            var destination = await _unitOfWork.Destinations.GetOneAsync(match, includes);

            if (destination is null)
                return null;

            var destinationDTO = new GetDestinationDTO
            {
                Id = destination.Id,
                Description = destination.Description,
                DestinationCategory = destination.DestinationCategory.ToString(),
                Name = destination.Name,
                ImagePath = destination.Images.Select(i => i.ImagePath).ToList()
            };

            return destinationDTO;
        }

        public async Task<ResponseModel> AddDestinationAsync(AddDestinationDTO model)
        {
            try
            {
                Destination newDestination = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    DestinationCategory = model.DestinationCategory,
                };

                await _unitOfWork.Destinations.AddAsync(newDestination);
                await _unitOfWork.SaveChangesAsync();

                if (model.Images != null && model.Images.Any())
                {
                    var response = await AddImageAsync(newDestination, model.Images);
                    if (response.StatusCode == 200)
                        response.Message = "Destination successfully added with all images.";
                    return response;
                }

                return new ResponseModel { Message = "Destination has been added successfully without any image.", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> UpdateDestinationAsync(int destinationID, UpdateDestinationDTO model)
        {
            try
            {
                Expression<Func<Destination, bool>> predicate = d => d.Id == destinationID;
                var destination = await _unitOfWork.Destinations.GetOneAsync(predicate);
                if (destination is null)
                    return new ResponseModel { StatusCode = 400, Message = "this Destination not exist." };
                destination.Name = model.Name;
                destination.DestinationCategory = model.DestinationCategory;
                destination.Description = model.Description;
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "Destination has been updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred : {ex.Message}" };
            }

        }

        public async Task<ResponseModel> DeleteDestinationAsync(int DestinationID)
        {
            try
            {
                Expression<Func<Destination, bool>> predicate = d => d.Id == DestinationID;
                var includes = new List<Expression<Func<Destination, object>>> { e => e.Images };
                var destination = await _unitOfWork.Destinations.GetOneAsync(predicate, includes);
                if (destination is null)
                    return new ResponseModel { StatusCode = 400, Message = "this Destination not exist." };

                // Delete images 
                foreach (var image in destination.Images)
                {
                    string oldImagePath = Path.Combine("wwwroot", "Images_of_Destination", Path.GetFileName(image.ImagePath));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }
                _unitOfWork.DestinationImages.RemoveRange(destination.Images);
                _unitOfWork.Destinations.Remove(destination);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel { StatusCode = 200, Message = "destination has been deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred : {ex.Message}" };
            }
        }

        public async Task<List<GetDestinationDTO>> GetBasedOnCategoryAsync(Category category)
        {
            var includes = new List<Expression<Func<Destination, object>>> { o => o.Images };
            Expression<Func<Destination, bool>> match = o => o.DestinationCategory == category;
            var destinations = await _unitOfWork.Destinations.GetAllAsync(includes, match);

            return destinations.Select(x => new GetDestinationDTO
            {
                Id = x.Id,
                Description = x.Description,
                DestinationCategory = x.DestinationCategory.ToString(),
                Name = x.Name,
                ImagePath = x.Images.Select(i => i.ImagePath).ToList()
            }).ToList(); ;
        }

        public async Task<ResponseModel> AddImagesToDestinationAsync(int destinationID, List<IFormFile> images)
        {
            try
            {
                if (images != null && images.Any())
                {
                    Expression<Func<Destination, bool>> match = o => o.Id == destinationID;
                    var destination = await _unitOfWork.Destinations.GetOneAsync(match);
                    if (destination is null)
                        return new ResponseModel { Message = "this destination not exist.", StatusCode = 400 };

                    return await AddImageAsync(destination, images);
                }

                return new ResponseModel { StatusCode = 400, Message = "not exist attached images." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred : {ex.Message} " };
            }
        }

        public async Task<List<string>> GetImagesOfDestinationAsync(int DestinationID)
        {
            var includes = new List<Expression<Func<Destination, object>>> { o => o.Images };
            Expression<Func<Destination, bool>> match = o => o.Id == DestinationID;
            var destination = await _unitOfWork.Destinations.GetOneAsync(match, includes);
            if (destination is null)
                return new List<string>();

            return destination.Images.Select(i => i.ImagePath).ToList();
        }

        public async Task<ResponseModel> UpdateImagesOfDestinationAsync(int DestinationID, List<IFormFile> images)
        {
            if (images == null || !images.Any())
                return new ResponseModel { StatusCode = 400, Message = "Not found images , Enter at least one image." };

            var includes = new List<Expression<Func<Destination, object>>> { o => o.Images };
            Expression<Func<Destination, bool>> match = o => o.Id == DestinationID;
            var destination = await _unitOfWork.Destinations.GetOneAsync(match, includes);
            if (destination is null)
                return new ResponseModel { StatusCode = 400, Message = "this Destination not exist." };
            try
            {
                // Delete old images 
                foreach (var image in destination.Images)
                {
                    string oldImagePath = Path.Combine("wwwroot", "Images_of_Destination", Path.GetFileName(image.ImagePath));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }
                _unitOfWork.DestinationImages.RemoveRange(destination.Images);
                await _unitOfWork.SaveChangesAsync();

                //add the new iamegs
                return await AddImageAsync(destination, images);
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred : {ex.Message}" };
            }
        }


        private async Task<ResponseModel> AddImageAsync(Destination destination, List<IFormFile> images)
        {
            try
            {
                string imageDirectory = Path.Combine("wwwroot", "Images_of_Destination");

                if (!Directory.Exists(imageDirectory))
                    Directory.CreateDirectory(imageDirectory);

                List<string> invalidFiles = new();
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                foreach (var formFile in images)
                {
                    var extension = Path.GetExtension(formFile.FileName)?.ToLowerInvariant();

                    if (formFile.Length <= 5 * 1024 * 1024 && allowedExtensions.Contains(extension))
                    {
                        string fileName = $"{destination.Name}_{Guid.NewGuid()}.jpg";
                        string filePath = Path.Combine(imageDirectory, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        string imageUrl = $"{_urlHelperService.GetCurrentServerUrl()}/Images_of_Destination/{fileName}";

                        await _unitOfWork.DestinationImages.AddAsync(new DestinationImages
                        {
                            ImagePath = imageUrl,
                            DestinationId = destination.Id,
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
