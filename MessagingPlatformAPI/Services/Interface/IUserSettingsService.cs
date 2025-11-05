using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.SettingsDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IUserSettingsService
    {
        Task<UserSettings> GetById(string UserId);
        Task<SimpleResponseDTO> Create(string UserId);
        Task<SimpleResponseDTO> Update(Guid SettingsId, UpdateUserSettingsDTO model);
    }
}
