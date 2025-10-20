using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IUserConnectionService
    {
        Task<UserConnection> GetByConnectionId(string ConnectionId);
        Task<SimpleResponseDTO> Create(string connectionId);
        Task<SimpleResponseDTO> Delete(string connectionId);
    }
}
