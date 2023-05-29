using Microsoft.AspNetCore.Identity;
using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;

namespace WebAPI.Models.Interfaces
{
    public interface IAccountService
    {
        Task<bool> ChangePassword(ChangePasswordSchema schema, string email);
        Task<bool> ChangePassword(RecoverPasswordSchema schema);
        Task<ConfirmPhoneDTO> ConfirmPhone(string phoneNo, string email);
        Task<UserProfileDTO> GetProfile(string userName);
        Task<string> GetUserIdAsync(string userName);
        Task<string> LogInAsync(LoginAccountSchema schema);
        Task<string> LogInExternalAsync(ExternalLoginInfo externalUser);
        Task LogOutAsync();
        Task<bool> RegisterAsync(RegisterAccountSchema schema);
        Task<bool> ResetPassword(string email);
        Task<UserProfileDTO> ReturnProfileAsync(string Id);
        Task<UserProfileDTO> UpdateProfileAsync(UpdateUserSchema schema, string userName);
        Task<bool> VerifyPhone(string email);
        Task<bool> AddPhoneNumberToUser(string phoneNumber, string email);
        Task<bool> DeleteProfile(string userName);
        Task<bool> DeleteUser(string id);
    }
}