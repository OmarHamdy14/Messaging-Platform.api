using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.Enums;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IEntityBaseRepository<Message> _base;
        private readonly IChatService _chatService;
        private readonly IAccountService _accountService;
        private readonly IHubContext _hubContext;
        private readonly IMapper _mapper;
        public MessageService(IEntityBaseRepository<Message> @base, IMapper mapper, IChatService chatService, IHubContext hubContext, IAccountService accountService)
        {
            _base = @base;
            _mapper = mapper;
            _chatService = chatService;
            _hubContext = hubContext;
            _accountService = accountService;
        }
        public async Task<List<Message>> GetAllByChatId(Guid ChatId)
        {
            return await _base.GetAll(p => p.ChatId == ChatId);
        }
       /* public async Task<List<Message>> GetAllByPrivateChatId(Guid PrivateChatId)
        {
            return await _base.GetAll(p => p.PrivateChatId == PrivateChatId);
        }
        public async Task<List<Message>> GetAllByGroupChatId(Guid GroupChatId)
        {
            return await _base.GetAll(p => p.GroupChatId == GroupChatId);
        }*/
        public async Task<SimpleResponseDTO> Create(CreateMessageDTO model)
        {
            var message = _mapper.Map<Message>(model);
            var chat = await _chatService.GetById(model.ChatId);
            var allMembers = chat.Members.Where(c => c.MemberId != model.UserId); // all except sender
            foreach (var cm in allMembers)
            {
                var reciever = await _accountService.FindById(cm.MemberId);
                message.messageStatuses.Add(new Models.MessageStatus()
                {
                    MessageId = message.Id,
                    RecieverId = cm.MemberId,
                    status = reciever.IsOnline ? Helpers.Enums.MessageStatusEnum.delivered : Helpers.Enums.MessageStatusEnum.sent,
                    UpdatedAt = DateTime.Now
                });
            }
            await _base.Create(message);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Message creation is done" };
        }
        public async Task<SimpleResponseDTO> Update(Guid MessageId, UpdateMessageDTO model)
        {
            var message = await _base.Get(c => c.Id == MessageId);
            if (message == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Message is not found" };
            _mapper.Map(message, model);
            message.EditedAt = DateTime.UtcNow;
            message.IsEdited = true;
            await _base.Update(message);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Message Update is done" };
        }
        public async Task<SimpleResponseDTO> Delete(Guid MessageId)
        {
            var message = await _base.Get(c => c.Id == MessageId);
            if (message == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Message is not found" };
            message.DeletedAt = DateTime.UtcNow;
            message.IsDeleted = true;
            await _base.Update(message);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Message deletion is done" };
        }
    }
}
