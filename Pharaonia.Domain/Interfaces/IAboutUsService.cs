namespace Pharaonia.Domain.Interfaces
{
    public interface IAboutUsService
    {
        Task<string?> GetAboutUsAsync();
        Task<ResponseModel> AddAboutUsAsync(AboutUsDTO model);
        Task<ResponseModel> UpdateAboutUsAsync(AboutUsDTO model);
        Task<ResponseModel> DeleteAboutUsAsync();
    }
}
