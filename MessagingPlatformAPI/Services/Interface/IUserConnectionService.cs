using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.UserConnectionDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IUserConnectionService
    {
        Task<UserConnection> GetByConnectionId(string ConnectionId);
        Task<SimpleResponseDTO> Create(CreateUserConnectionDTO model);
        Task<SimpleResponseDTO> Delete(string connectionId);
    }
}
