

namespace Pharaonia.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel> RegistrationAsAdminAsync(RegisterDTO data);
        Task<ResponseModel> ConfirmEmailAsync(string userId, string token);
        Task<ResponseModel> LoginAsync(LoginDTO data);
        public Task<ResponseModel> ForgotPasswordAsync(string email);
        public Task<ResponseModel> VerifyOTPAsync(VerifyOTPDTO model);
        public Task<ResponseModel> ResetPasswordAsync(ResetPasswordDTO model);

    }
}
