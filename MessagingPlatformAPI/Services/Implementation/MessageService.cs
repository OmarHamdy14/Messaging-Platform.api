using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.MessageDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IEntityBaseRepository<Message> _base;
        private readonly IChatService _chatService;
        private readonly IHubContext _hubContext;
        private readonly IMapper _mapper;
        public MessageService(IEntityBaseRepository<Message> @base, IMapper mapper, IChatService chatService, IHubContext hubContext)
        {
            _base = @base;
            _mapper = mapper;
            _chatService = chatService;
            _hubContext = hubContext;
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
            await _base.Create(message);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Message creation is done" };
        }
        public async Task<SimpleResponseDTO> Update(Guid MessageId, UpdateMessageDTO model)
        {
            var message = await _base.Get(c => c.Id == MessageId);
            if (message == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Message is not found" };
            _mapper.Map(message, model);
            await _base.Update(message);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Message Update is done" };
        }
        public async Task<SimpleResponseDTO> Delete(Guid MessageId)
        {
            var message = await _base.Get(c => c.Id == MessageId);
            if (message == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Message is not found" };
            message.IsDeleted = true;
            await _base.Update(message);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Message deletion is done" };
        }
    }
}
