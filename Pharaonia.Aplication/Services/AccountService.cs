
using Microsoft.AspNetCore.Identity;
using Pharaonia.Domain.DTOs;

namespace Pharaonia.Aplication.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly JwtOptions _jwtOptions;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, JwtOptions jwtOptions, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _jwtOptions = jwtOptions;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModel> RegistrationAsAdminAsync(RegisterDTO data)
        {
            ResponseModel response = new ResponseModel();
            User? findUser = await _userManager.FindByEmailAsync(data.Email);
            if (findUser != null)
            {
                response.Message = "This email is not available";
                response.StatusCode = 400;
                return response;
            }

            var identityUser = new User
            {
                UserName = data.Name,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(identityUser, data.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                response.Message = errors;
                response.StatusCode = 400;
                return response;
            }

            //Add Admin
            IdentityResult roleResult = await _userManager.AddToRoleAsync(identityUser, "Admin");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(identityUser); // Rollback user creation
                var errors = string.Empty;
                foreach (var error in roleResult.Errors)
                    errors += $"{error.Description},";

                response.Message = errors;
                response.StatusCode = 400;
                return response;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            token = WebUtility.UrlEncode(token);

            // Create confirmation link 
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";
            var confirmationLink = $"{baseUrl}/ConfirmEmail?userId={identityUser.Id}&token={token}";

            // Email body with confirmation link
            string body = $@"
<div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 40px; text-align: center;'>
    <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 30px; border-radius: 12px; 
                box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1); overflow: hidden;'>

        <!-- Header -->
        <div style='background: linear-gradient(90deg, #6a11cb, #2575fc); padding: 25px; border-radius: 12px 12px 0 0;'>
            <h2 style='color: #fff; font-size: 28px; font-weight: bold; margin: 0; letter-spacing: 1px;'>🎉 Welcome to Pharaonia! 🎉</h2>
        </div>

        <!-- Content -->
        <div style='padding: 30px; text-align: left;'>
            <p style='font-size: 20px; color: #333; font-weight: bold; margin-bottom: 10px;'>Hello {data.Name},</p>
            <p style='font-size: 18px; color: #555; line-height: 1.6;'>
                We're thrilled to have you as an <span style='color: #6a11cb; font-weight: bold;'>Admin</span> at Pharaonia! 🎊 
                To complete your registration, please confirm your email address by clicking the button below:
            </p>

            <!-- Button -->
            <div style='text-align: center; margin: 30px 0;'>
                <a href='{confirmationLink}' 
                   style='display: inline-block; background: linear-gradient(90deg, #4CAF50, #43A047); color: white; padding: 16px 40px; 
                          text-decoration: none; font-size: 18px; font-weight: bold; border-radius: 8px; 
                          box-shadow: 0 5px 10px rgba(0, 0, 0, 0.2); transition: all 0.3s ease;'>
                    ✅ Confirm Email
                </a>
            </div>

            <p style='font-size: 16px; color: #555; margin-top: 20px;'>Best Regards,</p>
            <p style='font-size: 18px; color: #333; font-weight: bold;'>🌟 The Pharaonia Team</p>
        </div>

        <!-- Footer -->
        <div style='background: #f4f4f4; padding: 20px; border-radius: 0 0 12px 12px;'>
            <p style='font-size: 14px; color: #777; margin: 0;'>If you didn't request this, please ignore this email.</p>
            <p style='font-size: 14px; color: #777; margin: 5px 0;'>Need help? <a href='https://wa.me/+201026408604' style='color: #6a11cb; text-decoration: none;'>Contact Support</a></p>
        </div>
    </div>
</div>

<style>
    a:hover {{
        background: linear-gradient(90deg, #43A047, #4CAF50);
        transform: scale(1.05);
        transition: all 0.3s ease-in-out;
    }}
</style>";





            // Send confirmation email
            await _emailSender.SendEmailAsync(identityUser.Email, "Confirm your email address", body);

            response.Message = "Admin created successfully , Please check your email to confirm your account.";
            response.StatusCode = 200;
            return response;

        }


        public async Task<ResponseModel> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return new ResponseModel
                {
                    Message = "Invalid email confirmation request.",
                    StatusCode = 400
                };
            }

            token = WebUtility.UrlDecode(token); // Decode the token

            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseModel
                {
                    Message = "User not found ,Register again.",
                    StatusCode = 404
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Message = "Email confirmed successfully, Your account is now activated , You can login now.",
                    StatusCode = 200
                };
            }

            // If confirmation fails, delete the user
            var deleteResult = await _userManager.DeleteAsync(user);

            if (deleteResult.Succeeded)
            {
                return new ResponseModel
                {
                    Message = "Email confirmation failed, please register again.",
                    StatusCode = 400
                };
            }

            // If user deletion fails, return an error message
            string deleteErrors = string.Join("; ", deleteResult.Errors.Select(e => e.Description));

            return new ResponseModel
            {
                Message = $"Email confirmation failed, and we couldn't delete your account: {deleteErrors}",
                StatusCode = 500
            };
        }


        public async Task<ResponseModel> LoginAsync(LoginDTO data)
        {
            var user = await _userManager.FindByEmailAsync(data.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, data.Password))
                return new ResponseModel { Message = "Email or password is Incorrect.", StatusCode = 400 };

            if (!user.EmailConfirmed)
                return new ResponseModel { Message = "please activate your account before Login.", StatusCode = 400 };

            var jwtSecurityToken = await CreateJwtTokenAsync(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            return new ResponseModel { StatusCode = 200, Message = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken) };
        }


        private async Task<JwtSecurityToken> CreateJwtTokenAsync(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }.Union(userClaims).Union(roleClaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.LifeTime),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        #region ForgetPassword
        public async Task<ResponseModel> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ResponseModel { Message = "User Not Found", StatusCode = 400 };

            var otp =await GenerateOTP(user);
            try
            {
                var to = new List<string> { email };
                var subject = "Reset Your Password";
                var body = $@"
                               <!DOCTYPE html>
                               <html>
                               <head>
                                   <style>
                                       body {{
                                           font-family: Arial, sans-serif;
                                           background-color: #f4f4f4;
                                           margin: 0;
                                           padding: 0;
                                       }}
                                       .container {{
                                           max-width: 500px;
                                           margin: 20px auto;
                                           background-color: #ffffff;
                                           padding: 20px;
                                           border-radius: 10px;
                                           box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
                                           text-align: center;
                                       }}
                                       h1 {{
                                           color: #333;
                                           font-size: 24px;
                                       }}
                                       p {{
                                           font-size: 16px;
                                           color: #555;
                                           margin: 10px 0;
                                       }}
                                       .otp-code {{
                                           font-size: 24px;
                                           font-weight: bold;
                                           color: #ff5722;
                                           background: #ffebee;
                                           padding: 10px;
                                           display: inline-block;
                                           border-radius: 5px;
                                           margin: 10px 0;
                                       }}
                                       .footer {{
                                           font-size: 14px;
                                           color: #888;
                                           margin-top: 20px;
                                       }}
                                   </style>
                               </head>
                               <body>
                                   <div class='container'>
                                       <h1>🔐 Reset Your Password</h1>
                                       <p>Hello,{user.UserName}</p>
                                       <p>You have requested to reset your password. Use the OTP below to proceed:</p>
                                       <div class='otp-code'>{otp}</div>
                                       <p>This OTP is valid for <strong>5 minutes</strong>. Do not share it with anyone.</p>
                                       <p>If you didn't request this, please ignore this email.</p>
                                       <div class='footer'>
                                           <p>📩 Need help? <a href='https://wa.me/+201026408604'>Contact our support team.</a></p>
                                       </div>
                                   </div>
                               </body>
                               </html>";
                               

                await _emailSender.SendEmailAsync(email, subject, body);

                return new ResponseModel { Message = "OTP Sent Successfully, please Check your gmail.", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Message = $"An Error Occurred , {ex.Message}", StatusCode = 500 };
            }
        }

        public async Task<string> GenerateOTP(User user)
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            user.OTPResetPassword = otp;
            user.OTPResetPasswordExpiration = DateTime.UtcNow.AddMinutes(5); // OTP expires in 5 minutes
            await _unitOfWork.SaveChangesAsync();
            return otp;
        }

        public async Task<ResponseModel> VerifyOTPAsync(VerifyOTPDTO Model)
        {
            if (! await VerifyOTPAsync(Model.Email, Model.OTP))
                return new ResponseModel { Message = "Invalid OTP", StatusCode = 400 };

            var user = await _userManager.FindByEmailAsync(Model.Email);
            if (user == null)
                return new ResponseModel { Message = "User Not Found", StatusCode = 400 };
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new ResponseModel { Message = token, StatusCode = 200 };
        }

        public async Task<bool> VerifyOTPAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            if (user.OTPResetPassword == otp && user.OTPResetPasswordExpiration > DateTime.UtcNow)
            {
                user.OTPResetPasswordExpiration = null;
                user.OTPResetPassword = null;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ResponseModel> ResetPasswordAsync(ResetPasswordDTO Model)
        {
            var user = await _userManager.FindByEmailAsync(Model.Email);
            if (user == null)
                return new ResponseModel { Message = "User Not Found", StatusCode = 400 };

            var token = Model.Token;
            if (token == null)
                return new ResponseModel { Message = "Invalid Token", StatusCode = 400 };

            if (Model.NewPassword != Model.ConfirmPassword)
                return new ResponseModel { Message = "Passwords Do Not Match", StatusCode = 400 };

            var result = await _userManager.ResetPasswordAsync(user, token, Model.NewPassword);
            string errorMessage = result.Errors.Any() ? result.Errors.First().Description : "Unknown error.";
            string msg = result.Succeeded ? "Password Reset Successfully" : $"Password Reset Failed{errorMessage}";
            int code = result.Succeeded ? 200 : 400;
            return new ResponseModel { Message = msg, StatusCode = code };
        }

        #endregion
    }
}
