
namespace Pharaonia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin"), HttpPost("/RegistrationAsAdmin")]
        public async Task<IActionResult> RegistrationAsAdmin([FromBody] RegisterDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResponseModel response = await _accountService.RegistrationAsAdminAsync(user);

            if (response == null)
                return BadRequest("An error occurred while processing your request.");

            if (response.StatusCode == 400)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }


        [HttpGet("/ConfirmEmail"), AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            ResponseModel response = await _accountService.ConfirmEmailAsync(userId, token);

            if (response == null)
                return BadRequest("An error occurred while processing your request.");

            if (response.StatusCode == 400)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }


        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.LoginAsync(user);

            if (response == null)
                return BadRequest("An error occurred while processing your request.");

            if (response.StatusCode == 400)
                return BadRequest(response.Message);

            return Ok(response.Message);

        }

    }
}
