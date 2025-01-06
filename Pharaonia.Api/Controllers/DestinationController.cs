
using Pharaonia.Domain.DTOs;
using Pharaonia.Domain.Models;

namespace Pharaonia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        [HttpGet("/Get-All-Destinations")]
        public async Task<IActionResult> GetAllAsync() 
        { 
          List<GetDestinationDTO> Response = await _destinationService.GetAllAsync();
            if(Response==null)
                return NotFound();
          return Ok(Response);
        }


        [HttpGet("/Get-Destination-By-Id")]
        public  async Task<IActionResult> GetByIdAsync(int id)
        {
            GetDestinationDTO? Response =await _destinationService.GetByIdAsync(id);
            if (Response == null)
                return BadRequest("enter a valid ID.");
            return Ok(Response);
        }


        [HttpGet("/Get-Destinations-Based-On-Category")]
        public  async Task<IActionResult> GetBasedOnCategoryAsync(Category category)
        {
            List<GetDestinationDTO> Response =await _destinationService.GetBasedOnCategoryAsync(category);
            return Ok(Response);
        }

        
        [HttpGet("/Get-Images-Of-Destination/{DestinationID:int}")]
        public async Task<IActionResult> GetImagesOfDestinationAsync([FromRoute] int DestinationID)
        {
            List<string> images = await _destinationService.GetImagesOfDestinationAsync(DestinationID);
            if (images is null)
                return BadRequest("Destination not found or has no images.");
            return Ok(images);
        }


        [HttpPost("/Add-Destination")]
        public async Task<IActionResult> AddDestinationAsync([FromForm]AddDestinationDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please enter a vaild Data.");
            var response = await _destinationService.AddDestinationAsync(model);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message), 
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }


        [HttpPost("/Add-Image-To-Destination/{DestinationID:int}")]
        public async Task<IActionResult> AddIamgeToDestinationAsync ([FromRoute]int DestinationID, [FromForm] List<IFormFile> images)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please enter a vaild Data.");
            var response = await _destinationService.AddImagesToDestinationAsync(DestinationID, images);
            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message), 
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }


        [HttpPut("/Update-Images-Of-Destination/{DestinationID:int}")]
        public async Task<IActionResult> UpdateImagesOfDestinationAsync([FromRoute] int DestinationID, [FromForm] List<IFormFile> images)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please enter a vaild Data.");
            var response = await _destinationService.UpdateImagesOfDestinationAsync(DestinationID, images);
            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }


        [HttpPut("/Update-Destination{DestinationID:int}")]
        public async Task<IActionResult> UpdateDestinationAsync([FromRoute]int DestinationID,[FromBody] UpdateDestinationDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please enter a vaild Data.");
            var response = await _destinationService.UpdateDestinationAsync(DestinationID, model);
            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }


        [HttpDelete("/Delete-Destination{DestinationID:int}")]
        public async Task<IActionResult> DeleteDestinationAsync([FromRoute]int DestinationID)
        {
            var response = await _destinationService.DeleteDestinationAsync(DestinationID);
            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }

    }
}
