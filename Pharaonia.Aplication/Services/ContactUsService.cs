
namespace Pharaonia.Aplication.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactUsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModel> AddAsync(AddContactUSDTO model)
        {
            try
            {
                await _unitOfWork.ContactUS.AddAsync(new ContactUS { CreatedTime = DateTime.Now, Email = model.Email, Message = model.Message, Name = model.Name, Phone = model.Phone });
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { Message = "ContactUs has been added successfully.", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while adding ContactUs: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteAsync(int id)
        {
            try
            { 
                var old = await GetByIdAsync(id);
                if (old == null)
                    return new ResponseModel { Message = "Not found this ContactUs.", StatusCode = 400 };
                _unitOfWork.ContactUS.Remove(old);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel { Message = "ContactUs has been Deleted successfully.", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel { StatusCode = 500, Message = $"An error occurred while Deleting ContactUs: {ex.Message}" };
            }
        }

        public async Task<List<ContactUS>> GetAllAsync()
        {
            var data = await _unitOfWork.ContactUS.GetAllAsync();
            return data.ToList();
        }

        public Task<ContactUS?> GetByIdAsync(int id)
        {
            Expression<Func<ContactUS, bool>> match = e => e.Id == id;
            var data = _unitOfWork.ContactUS.GetOneAsync(match);
            if (data == null)
                return null;
            return data;
        }
    }
}
