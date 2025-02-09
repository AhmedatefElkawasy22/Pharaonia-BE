
using Azure;

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


        #region forgetPassword

        [HttpPost("Forgot-Password/{email}")]
        public async Task<IActionResult> ForgotPassword([FromRoute]string email)
        {
            var response = await _accountService.ForgotPasswordAsync(email);
            return response.StatusCode switch
            {
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }

        [HttpPost("Verify-Otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.VerifyOTPAsync(model);
            return response.StatusCode switch
            {
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _accountService.ResetPasswordAsync(model);
            return response.StatusCode switch
            {
                400 => BadRequest(response.Message),
                200 => Ok(response.Message),
                500 => StatusCode(500, response.Message),
                _ => StatusCode(500, "An unexpected error occurred , try again."),
            };
        } 
        #endregion

    }
}
