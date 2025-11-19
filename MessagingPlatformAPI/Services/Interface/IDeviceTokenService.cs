using MessagingPlatformAPI.Helpers.DTOs.DeviceTokenDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IDeviceTokenService
    {
        Task<SimpleResponseDTO<DeviceToken>> CreateDeviceToken(string UserId, CreateTokenDTO model);
        Task DeleteAsync(DeviceToken token);
    }
}
