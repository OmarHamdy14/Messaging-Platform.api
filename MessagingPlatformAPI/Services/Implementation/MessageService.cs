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
        private readonly IEntityBaseRepository<MessageImage> _messageImageBase;
        private readonly IChatService _chatService;
        private readonly IAccountService _accountService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IHubContext _hubContext;
        private readonly IMapper _mapper;
        public MessageService(IEntityBaseRepository<Message> @base, IMapper mapper, IChatService chatService, IHubContext hubContext, IAccountService accountService, ICloudinaryService cloudinaryService, IEntityBaseRepository<MessageImage> messageImageBase)
        {
            _base = @base;
            _mapper = mapper;
            _chatService = chatService;
            _hubContext = hubContext;
            _accountService = accountService;
            _cloudinaryService = cloudinaryService;
            _messageImageBase = messageImageBase;
        }
        public async Task<Message> GetById(Guid MessageId)
        {
            return await _base.Get(m => m.Id == MessageId); 
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
        public async Task<SimpleResponseDTO> Create(CreateMessageDTO model, List<IFormFile> files)
        {
            var message = _mapper.Map<Message>(model);
            var chat = await _chatService.GetById(model.ChatId);
            var res = await _cloudinaryService.UploadFiles(files);
            if (!res.AllSucceeded) return new SimpleResponseDTO() { IsSuccess = false };
            await _base.Create(message);
            foreach (var response in res.UploadedPhotos)
            {
                await _messageImageBase.Create(new MessageImage() { PublicId=response.PublicId, Url=response.Url, MessageId=message.Id});
            }
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
        public async Task<SimpleResponseDTO> DeleteMessagePic(string ImagePublicId)
        {
            var currentPic = await _messageImageBase.Get(p => p.PublicId == ImagePublicId);
            if (currentPic != null)
            {
                await _cloudinaryService.DeleteFile(ImagePublicId);
                await _messageImageBase.Remove(currentPic);
                return new SimpleResponseDTO() { IsSuccess = true, Message = "Deletion is done" };
            }
            return new SimpleResponseDTO() { IsSuccess = false, Message = "Deletion is failed" };
        }
    }
}
