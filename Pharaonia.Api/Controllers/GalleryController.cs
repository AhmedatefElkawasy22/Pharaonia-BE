using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pharaonia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet("/Get-All")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _galleryService.GetAllAsync());
        }
        [HttpGet("/Get-By-Id/{Id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int Id)
        {
            var Image = await _galleryService.GetByIdAsync(Id);
            if (Image == null) 
                return NotFound("Image Not Found.");

            return Ok(Image);
        }
        [HttpPost("Add-Images")]
        public async Task<IActionResult> AddImagesAsync([FromForm] List<IFormFile> images)
        {
            var response = await _galleryService.AddAsync(images);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }
        [HttpDelete("Delete-Image/{Id:int}")]
        public async Task<IActionResult> DeleteImageAsync([FromRoute] int Id)
        {
            var response = await _galleryService.DeleteAsync(Id);
            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }
        [HttpDelete("Delete-All-Images")]
        public async Task<IActionResult> DeleteAllImagesAsync()
        {
            var response = await _galleryService.DeleteAllAsync();
            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }
    }
}
