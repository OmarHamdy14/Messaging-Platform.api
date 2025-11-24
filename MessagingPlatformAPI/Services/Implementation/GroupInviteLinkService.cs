using AutoMapper;
using CloudinaryDotNet.Actions;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.GroupInviteLinkDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using System.ComponentModel;
using System.Security.Cryptography;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class GroupInviteLinkService : IGroupInviteLinkService
    {
        private readonly IEntityBaseRepository<GroupInviteLink> _base;
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        private readonly IChatMembersService _chatMembersServic;
        public GroupInviteLinkService(IEntityBaseRepository<GroupInviteLink> @base, IMapper mapper, IChatMembersService chatMembersServic, IChatService chatService)
        {
            _base = @base;
            _mapper = mapper;
            _chatMembersServic = chatMembersServic;
            _chatService = chatService;
        }
        public async Task<SimpleResponseDTO<GroupInviteLink>> CreateLink(CreateLinkDTO model)
        {
            var link = _mapper.Map<GroupInviteLink>(model);
            link.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            await _base.Create(link);
            return new SimpleResponseDTO<GroupInviteLink>() { IsSuccess = true, Object = link };
        }
        public async Task<bool> JoinGroup(JoinViaLinkDTO model)
        {
            var link = await _base.Get( l=> l.Token == model.token && l.ChatId == model.chatId);
            if (link == null) return false;
            if (link.IsRevoked) return false;
            if (link.ExpiresAt.HasValue && link.ExpiresAt.Value < DateTime.UtcNow) return false;
            if (link.MaxUses.HasValue && link.UsesCount >= link.MaxUses.Value) return false;

            link.UsesCount++;
            await _base.Update(link);
            var member = new Chat_Member() { ChatId = model.chatId, MemberId = model.userId };
            var chat = await _chatService.GetById(model.chatId);
            chat.Members.Add(member);
            return true;
        }
    }
}
