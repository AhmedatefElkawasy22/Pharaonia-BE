

namespace Pharaonia.Domain.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel> RegistrationAsAdminAsync(RegisterDTO data);
        Task<ResponseModel> ConfirmEmailAsync(string userId, string token);
        Task<ResponseModel> LoginAsync(LoginDTO data);

    }
}
