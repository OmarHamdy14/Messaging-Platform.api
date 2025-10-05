using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class ChatMembersService : IChatMembersService
    {
        private readonly IEntityBaseRepository<Chat_Member> _base;
        public ChatMembersService(IEntityBaseRepository<Chat_Member> @base)
        {
            _base = @base;
        }
        public async Task<List<Chat_Member>> GetAllByUserId(string UserId)
        {
            return await _base.GetAll(c => c.MemberId == UserId);
        }
        public async Task<List<Chat_Member>> GetAllByChatId(Guid ChatId)
        {
            return await _base.GetAll(c => c.ChatId == ChatId);
        }
        public async Task<SimpleResponseDTO> Delete(string UserId, Guid ChatId)
        {
            var cm = await _base.Get(c => c.ChatId == ChatId && c.MemberId == UserId);
            await _base.Remove(cm);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Deletion is succedded" };
        }
    }
}
