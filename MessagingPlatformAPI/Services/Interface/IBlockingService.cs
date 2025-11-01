using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IBlockingService
    {
        Task<bool> IsBlocked(string blockerId, string blockedId);
        Task<BlockedUser> GetByRecordId(Guid recId);
        Task<BlockedUser> GetByBlockerIdAndBlodkedId(string blockerId, string blockedId);
        Task<List<BlockedUser>> GetAllByBlockerId(string blockerId);
        Task<SimpleResponseDTO> Create(string blockerId, string blockedId);
        Task<SimpleResponseDTO> Delete(Guid recId);
    }
}
