using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.SettingsDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IUserSettingsService
    {
        Task<UserSettings> GetByUserId(string UserId);
        Task<SimpleResponseDTO<UserSettings>> Create(string UserId);
        Task<SimpleResponseDTO<UserSettings>> Update(Guid SettingsId, UpdateUserSettingsDTO model);
    }
}
