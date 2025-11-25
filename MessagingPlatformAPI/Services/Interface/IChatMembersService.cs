using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ChatMemberDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IChatMembersService
    {
        Task<Chat_Member> GetByChatIdAndMemberId(Guid chatId, string memberId);
        Task<List<Chat_Member>> GetAllByUserId(string UserId);
        Task<List<Chat_Member>> GetAllByChatId(Guid ChatId);
        Task<SimpleResponseDTO<Chat_Member>> MakeAdmin(RecordDTO model);
        Task<SimpleResponseDTO<Chat_Member>> Delete(RecordDTO model);

        Task<bool> MuteChat(MuteDTO model);
        Task<bool> MuteChatAlways(Guid chatId, string userId);
        Task<bool> UnMute(Guid chatId, string userId);
        Task<bool> IsMuted(Guid chatId, string userId);
    }
}