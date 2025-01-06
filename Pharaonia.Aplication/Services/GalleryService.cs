


namespace Pharaonia.Aplication.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlHelperService _urlHelperService;

        public GalleryService(IUnitOfWork unitOfWork, IUrlHelperService urlHelperService)
        {
            _unitOfWork = unitOfWork;
            _urlHelperService = urlHelperService;
        }

        public async Task<ResponseModel> AddAsync(List<IFormFile> images)
        {
            if (images == null || !images.Any())
                return new ResponseModel { Message = "Not Found images , please enter at least one image.", StatusCode = 400 };
            try
            {
                return await AddImageAsync(images);
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while adding images: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteAsync(int id)
        {
            try
            {
                var image = await GetByIdAsync(id);
                if (image == null)
                    return new ResponseModel { Message = "this image not found.", StatusCode = 400 };
                _unitOfWork.Gallery.Remove(image);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { Message = "Image has been Deleted successfully.", StatusCode = 200 };
            }
            catch (Exception ex) 
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while Delete the image: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteAllAsync()
        {
            try
            {
                var images = await GetAllAsync();
                if (images == null || !images.Any())
                    return new ResponseModel { Message = "The gallery is already empty.", StatusCode = 400 };
                _unitOfWork.Gallery.RemoveRange(images);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { Message = "Images has been Deleted successfully.", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while Delete images: {ex.Message}" };
            }
        }

        public async Task<IEnumerable<Gallery>> GetAllAsync()
        {
            var data = await _unitOfWork.Gallery.GetAllAsync();
            if (data == null || !data.Any())
                return Enumerable.Empty<Gallery>();
            return data;
        }

        public async Task<Gallery?> GetByIdAsync(int id)
        {
            Expression<Func<Gallery, bool>> match = e => e.Id == id;
            var data = await _unitOfWork.Gallery.GetOneAsync(match);
            if (data == null)
                return null;
            return data;
        }

        private async Task<ResponseModel> AddImageAsync(List<IFormFile> images)
        {
            try
            {
                string imageDirectory = Path.Combine("wwwroot", "Gallery");

                if (!Directory.Exists(imageDirectory))
                    Directory.CreateDirectory(imageDirectory);

                List<string> invalidFiles = new();

                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };

                foreach (var formFile in images)
                {
                    var extension = Path.GetExtension(formFile.FileName)?.ToLowerInvariant();
                    if (formFile.Length <= 5 * 1024 * 1024 && allowedExtensions.Contains(extension))
                    {
                        string fileName = $"Gallery_{Guid.NewGuid()}.jpg";
                        string filePath = Path.Combine(imageDirectory, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        string imageUrl = $"{_urlHelperService.GetCurrentServerUrl()}/Gallery/{fileName}";

                        await _unitOfWork.Gallery.AddAsync(new Gallery { pathImage = imageUrl });
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
