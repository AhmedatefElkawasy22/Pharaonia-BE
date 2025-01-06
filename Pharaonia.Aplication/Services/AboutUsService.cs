
namespace Pharaonia.Aplication.Services
{
    public class AboutUsService : IAboutUsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AboutUsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<string?> GetAboutUsAsync()
        {
            var data = await _unitOfWork.AboutUs.FirstAsync();
            if (data != null)
                return data.Text;
            return null;
        }

        public async Task<ResponseModel> AddAboutUsAsync(AboutUsDTO model)
        {
            var old = await _unitOfWork.AboutUs.FirstAsync();
            if (old != null)
                return new ResponseModel { StatusCode = 400, Message = "About us already exists." };
            try
            {
                await _unitOfWork.AboutUs.AddAsync(new AboutUs { Text = model.Text });
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "About us has been added successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while adding AboutUs: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> UpdateAboutUsAsync(AboutUsDTO model)
        {
            var old = await _unitOfWork.AboutUs.FirstAsync();
            if (old == null)
                return new ResponseModel { StatusCode = 400, Message = "not found about us for updating  ,  please add a new about us." };
            try
            {
                old.Text = model.Text;
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "About us has been updated successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while udapte About us: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteAboutUsAsync()
        {
            var old = await _unitOfWork.AboutUs.FirstAsync();
            if (old == null)
                return new ResponseModel { StatusCode = 400, Message = "not found about us for Deleting." };
            try
            {
                _unitOfWork.AboutUs.Remove(old);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { StatusCode = 200, Message = "About us has been Deleted successfully." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while Delete About us: {ex.Message}" };
            }
        }


    }
}
