
namespace Pharaonia.API.Controllers
{
    [Authorize(Roles = "Admin"), Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;

        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }

        [HttpGet("/Get-All-ContactUS")]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await _contactUsService.GetAllAsync();
            if (res == null || !res.Any())
                return NotFound();
            return Ok(res);
        }


        [HttpGet("/Get-ContactUS-By-ID/{Id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int Id)
        {
            if (Id <= 0)
                return BadRequest("Invalid offer ID.");

            var res = await _contactUsService.GetByIdAsync(Id);
            if (res == null)
                return NotFound();
            return Ok(res);
        }


        [HttpDelete("/Delete-ContactUS/{Id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int Id)
        {
            if (Id <= 0)
                return BadRequest("Invalid ID.");

            var response = await _contactUsService.DeleteAsync(Id);

            return response.StatusCode switch
            {
                206 => StatusCode(206, response.Message),
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred, please try again."),
            };
        }


        [AllowAnonymous,HttpPost("/Add-ContactUS")]
        public async Task<IActionResult> AddAsync([FromBody] AddContactUSDTO model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _contactUsService.AddAsync(model);

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
