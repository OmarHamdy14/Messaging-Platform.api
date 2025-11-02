using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ChatMemberDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using System.Diagnostics.Metrics;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class ChatMembersService : IChatMembersService
    {
        private readonly IEntityBaseRepository<Chat_Member> _base;
        public ChatMembersService(IEntityBaseRepository<Chat_Member> @base)
        {
            _base = @base;
        }
        public async Task<SimpleResponseDTO> MakeAdmin(RecordDTO model)
        {
            var member = await _base.Get(cm => cm.MemberId==model.MemberId && cm.ChatId == model.ChatId);
            if(member == null) return new SimpleResponseDTO() { IsSuccess = false, Message = $"Failed !!! This record: MemberId={model.MemberId} & ChatId={model.ChatId} is not found"};
            member.IsAdmin = true;
            await _base.Update(member);
            return new SimpleResponseDTO() { IsSuccess = true, Message = $"Succeeded !!! Making This record: MemberId={model.MemberId} & ChatId={model.ChatId} as a admin is done" };
        }
        public async Task<Chat_Member> GetByChatIdAndMemberId(Guid chatId, string memberId)
        {
            return await _base.Get(cm => cm.ChatId == chatId && cm.MemberId == memberId);
        }
        public async Task<List<Chat_Member>> GetAllByUserId(string UserId)
        {
            return await _base.GetAll(c => c.MemberId == UserId);
        }
        public async Task<List<Chat_Member>> GetAllByChatId(Guid ChatId)
        {
            return await _base.GetAll(c => c.ChatId == ChatId);
        }
        public async Task<SimpleResponseDTO> Delete(RecordDTO model)
        {
            var member = await _base.Get(c => c.ChatId == model.ChatId && c.MemberId == model.MemberId);
            if (member == null) return new SimpleResponseDTO() { IsSuccess = false, Message = $"Failed !!! This record: MemberId={model.MemberId} & ChatId={model.ChatId} is not found" };
            await _base.Remove(member);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Deletion is succedded" };
        }
    }
}
