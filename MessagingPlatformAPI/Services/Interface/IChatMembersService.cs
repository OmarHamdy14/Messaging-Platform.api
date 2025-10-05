using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IChatMembersService
    {
        Task<List<Chat_Member>> GetAllByUserId(string UserId);
        Task<List<Chat_Member>> GetAllByChatId(Guid ChatId);
        Task<SimpleResponseDTO> Delete(string UserId, Guid ChatId);
    }
}
