using Elagy.DTOs;
using Elagy.Models;

namespace Elagy.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel> RegisterAsync(RegisterUserDTO data);
        Task<ResponseModel> RegistrationAsAdminAsync(RegisterUserDTO data);
        Task<ResponseModel> RegistrationAsPharmacyAsync(RegisterUserDTO data);
        Task<ResponseModel> LoginAsync(LoginUserDTo data);

       
    }
}
