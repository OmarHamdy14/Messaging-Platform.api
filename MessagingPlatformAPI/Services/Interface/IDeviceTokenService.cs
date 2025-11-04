using MessagingPlatformAPI.Helpers.DTOs.DeviceTokenDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IDeviceTokenService
    {
        Task<SimpleResponseDTO> CreateDeviceToken(string UserId, CreateTokenDTO model);
    }
}
