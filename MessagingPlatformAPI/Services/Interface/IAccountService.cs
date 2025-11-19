using MessagingPlatformAPI.Helpers.DTOs.AccountDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.TokenDTOs;
using MessagingPlatformAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IAccountService
    {
        Task<ApplicationUser> FindById(string userId);
        Task<ApplicationUser> FindByEmail(string email);
        Task<ApplicationUser> FindByUserName(string name);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<AuthModel> Register(RegisterationDTO model, IFormFile profilePic);
        Task<AuthModel> GetTokenAsync(LogInDTO model);
        Task<IdentityResult> Update(ApplicationUser user, UpdateUserDTO model);
        Task<bool> ChangePassword(ApplicationUser user, ChangePasswordDTO model);
        Task SaveChangesAsync(ApplicationUser user);
        Task<SimpleResponseDTO<ProfileImage>> ChangeProfilePic(ApplicationUser User, IFormFile pic);
        Task<SimpleResponseDTO<ProfileImage>> DeleteProfilePic(string ImagePublicId);


        Task<string> CreateRefreshToken();
        Task<SimpleResponseDTO<RefreshTokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO model);
    }
}
