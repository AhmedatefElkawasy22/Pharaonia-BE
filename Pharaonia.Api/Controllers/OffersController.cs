
using Pharaonia.Domain.DTOs;

namespace Pharaonia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }


        [HttpGet("/Get-All-Offers")]
        public async Task<IActionResult> GetAllOffers()
        {
            var offers = await _offerService.GetAllOffersAsync();
            if (offers == null)
                 return NotFound();
            return Ok(offers);
        }


        [HttpGet("/Get-All-Offers-Available")]
        public async Task<IActionResult> GetAvailableOffersAsync()
        {
            var offers = await _offerService.GetAvailableOffersAsync();
            if (offers == null)
                return NotFound();
            return Ok(offers);
        }


        [HttpGet("/Get-Images-Of-Offer/{OfferId:int}")]
        public async Task<IActionResult> GetImagesOfOfferAsync([FromRoute]int OfferId)
        {
            var res = await _offerService.GetImagesOfOfferAsync(OfferId);
            if (res == null)
                return NotFound();
            return Ok(res);
        }


        [HttpGet("/Get-Offer-By-Id/{OfferId:int}")]
        public async Task<IActionResult> GetOfferByIdAsync([FromRoute]int OfferId)
        {
            var offer =await _offerService.GetOfferByIdAsync(OfferId);
            if (offer == null)
                return BadRequest("Offer Not Exist.");
            return Ok(offer);
        }


        [HttpGet("/Get-Offers-Expired")]
        public async Task<IActionResult> GetOffersExpiredAsync()
        {
            var offers = await _offerService.GetOffersExpiredAsync();
            if (offers == null)
                return NotFound();
            return Ok(offers);
        }


        [HttpGet("/Reactivate-Offer/{offerId:int}")]
        public async Task<IActionResult> ReactivateOfferAsync([FromRoute] int offerId)
        {
            if (offerId <= 0)
                return BadRequest("Invalid offer ID.");

            var response = await _offerService.ReactivateOfferAsync(offerId);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpPost("/Add-Images-To-Offer/{offerId:int}")]
        public async Task<IActionResult> AddImagesToOfferAsync([FromRoute]int offerId, List<IFormFile> images)
        {
            if (offerId <= 0)
                return BadRequest("Invalid offer ID.");

            if (images == null || images.Count == 0)
                return BadRequest("Please upload at least one image.");

            var response = await _offerService.AddImagesToOfferAsync(offerId, images);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpPost("/Add-offer")]
        public async Task<IActionResult> AddOfferAsync([FromForm]AddOfferDTO model)
        {
            if (!ModelState.IsValid) return BadRequest("please enter a valid Data.");

            var response = await _offerService.AddOfferAsync(model);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpPut("/Update-Offer/{OfferId:int}")]
        public async Task<IActionResult> UpdateOfferAsync([FromRoute]int OfferId, [FromBody]UpdateOfferDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("please enter a valid Data.");

            var response = await _offerService.UpdateOfferAsync(OfferId, model);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpPut("/Update-Images-Of-Offer/{offerId:int}")]
        public async Task<IActionResult> UpdateImagesOfOfferAsync([FromRoute] int offerId, [FromForm] List<IFormFile> images)
        {
            if (offerId <= 0)
                return BadRequest("Invalid offer ID.");
            if (images == null || images.Count == 0 || !images.Any())
                return BadRequest("No images found, please enter at least one image.");

            var response = await _offerService.UpdateImagesOfOfferAsync(offerId,images);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpDelete("/Delete-Image-From-Offer/{ImageId:int}")]
        public  async Task<IActionResult> DeleteImageFromOfferAsync([FromRoute] int ImageId)
        {
            if (ImageId <= 0)
                return BadRequest("Invalid offer ID.");

            var response =await _offerService.DeleteImageFromOfferAsync(ImageId);
            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpDelete("/Delete-All-Image-From-Offer/{offerId:int}")]
        public async Task<IActionResult> DeleteAllImageFromOfferAsync([FromRoute] int offerId)
        {
            if (offerId <= 0)
                return BadRequest("Invalid offer ID.");

            var response = await _offerService.DeleteAllImageFromOfferAsync(offerId);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpDelete("/Delete-Offer/{offerId:int}")]
        public async Task<IActionResult> DeleteOfferAsync([FromRoute] int offerId)
        {
            if (offerId <= 0)
                return BadRequest("Invalid offer ID.");

            var response = await _offerService.DeleteOfferAsync(offerId);

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
