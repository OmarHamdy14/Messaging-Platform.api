using MessagingPlatformAPI.Helpers.DTOs.ChatMemberDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IChatMembersService
    {
        Task<List<Chat_Member>> GetAllByUserId(string UserId);
        Task<List<Chat_Member>> GetAllByChatId(Guid ChatId);
        Task<SimpleResponseDTO> MakeAdmin(RecordDTO model);
        Task<SimpleResponseDTO> Delete(RecordDTO model);
    }
}