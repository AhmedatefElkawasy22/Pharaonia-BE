

using System.Data;

namespace Pharaonia.API.Controllers
{
    [Authorize(Roles = "Admin"), Route("api/[controller]")]
    [ApiController]
    public class AboutUsController : ControllerBase
    {
        private readonly IAboutUsService _aboutUsService;

        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }

        [AllowAnonymous,HttpGet("/Get-AboutUS")]
        public async Task<IActionResult> GetAboutUsAsync()
        {
            var data = await _aboutUsService.GetAboutUsAsync();
            if (string.IsNullOrEmpty(data))
                return NotFound();
            return Ok(data);
        }

        [HttpPost("/Add-AboutUs")]
        public async Task<IActionResult> AddAboutUsAsync([FromBody] AboutUsDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _aboutUsService.AddAboutUsAsync(model);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpPut("/update-AboutUs")]
        public async Task<IActionResult> UpdateAboutUsAsync([FromBody] AboutUsDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _aboutUsService.UpdateAboutUsAsync(model);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [HttpDelete("/Delete-AboutUs")]
        public async Task<IActionResult> DeleteAboutUsAsync()
        {
            var response = await _aboutUsService.DeleteAboutUsAsync();

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
