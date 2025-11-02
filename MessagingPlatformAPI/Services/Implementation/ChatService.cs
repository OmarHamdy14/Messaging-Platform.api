using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class ChatService : IChatService
    {
        private readonly IEntityBaseRepository<Chat> _base;
        private readonly IEntityBaseRepository<ChatImage> _chatPicBase;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        public ChatService(IEntityBaseRepository<Chat> @base, IMapper mapper, IEntityBaseRepository<ChatImage> chatPicBase, ICloudinaryService cloudinaryService)
        {
            _base = @base;
            _mapper = mapper;
            _chatPicBase = chatPicBase;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<Chat> GetById(Guid Id)
        {
            return await _base.Get(c => c.Id == Id, "Messages,Members");
        }
        public async Task<SimpleResponseDTO> Create(CerateChatDTO model)
        {
            var chat = _mapper.Map<Chat>(model);
            List<Chat_Member> members = new List<Chat_Member>();
            foreach (var memberId in model.MmebersId)
            {
                members.Add(new Chat_Member() { MemberId = memberId });
            }
            chat.Members = members;
            await _base.Create(chat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Chat creation is done" };
        }
        public async Task<SimpleResponseDTO> Update(Guid ChaId, UpdateChatDTO model)
        {
            var chat = await _base.Get(c => c.Id == ChaId);
            if (chat == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Chat is not found" };
            _mapper.Map(chat, model);
            await _base.Update(chat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Chat update is done" };
        }
        public async Task<SimpleResponseDTO> AddUserToChat(Guid ChatId, string UserId)
        {
            var member = new Chat_Member() { ChatId = ChatId, MemberId = UserId };
            var chat = await _base.Get(c => c.Id == ChatId);
            chat.Members.Add(member);
            return new SimpleResponseDTO() { IsSuccess=true, Message="Adding is succedded" };
        }
        public async Task<SimpleResponseDTO> Delete(Guid ChaId)
        {
            var chat = await _base.Get(c => c.Id == ChaId);
            if (chat == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Chat is not found" };
            await _base.Remove(chat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Chat deletion is done" };
        }
        public async Task<SimpleResponseDTO> LeaveGroupChat(Guid ChatId)
        {
            var chat = await _base.Get(c => c.Id == ChatId);
            if (chat == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Chat is not found" };
            chat.IsLeft = true;
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Now .. you are not a member of this chat" };
        }

        public async Task SaveChanges(Chat c)
        {
            await _base.Update(c);
        }
        public async Task<SimpleResponseDTO> ChangeChatPic(Chat chat, IFormFile pic)
        {
            var currentPic = await _chatPicBase.Get(p => p.ChatId == chat.Id);
            if (currentPic != null)
            {
                await _cloudinaryService.DeleteFile(currentPic.PublicId);
                await _chatPicBase.Remove(currentPic);
            }
            var cloudinaryRes = await _cloudinaryService.UploadFile(pic);
            if (cloudinaryRes.IsSuccess)
            {
                await _chatPicBase.Create(new ChatImage() { PublicId = cloudinaryRes.PublicId, Url = cloudinaryRes.Url, ChatId = chat.Id });
                return new SimpleResponseDTO() { IsSuccess = true, Message = "Changing chat picture is done" };
            }
            return new SimpleResponseDTO() { IsSuccess = false };
        }
        public async Task<SimpleResponseDTO> DeleteChatPic(string ImagePublicId)
        {
            var currentPic = await _chatPicBase.Get(p => p.PublicId == ImagePublicId);
            if (currentPic != null)
            {
                await _cloudinaryService.DeleteFile(ImagePublicId);
                await _chatPicBase.Remove(currentPic);
                return new SimpleResponseDTO() { IsSuccess = true, Message = "Deletion is done" };
            }
            return new SimpleResponseDTO() { IsSuccess = false, Message = "Deletion is failed" };
        }
    }
}
