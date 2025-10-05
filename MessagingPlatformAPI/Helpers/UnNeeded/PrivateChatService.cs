using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.GroupChatDTO;
using MessagingPlatformAPI.Helpers.DTOs.PrivateChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Helpers.UnNeeded
{
    public class PrivateChatService : IPrivateChatService
    {
        private readonly IEntityBaseRepository<PrivateChat> _base;
        private readonly IEntityBaseRepository<Users_Private> _baseUsersPrivate;
        private readonly IMapper _mapper;
        public PrivateChatService(IEntityBaseRepository<PrivateChat> @base, IMapper mapper, IEntityBaseRepository<Users_Private> baseUsersPrivate)
        {
            _base = @base;
            _mapper = mapper;
            _baseUsersPrivate = baseUsersPrivate;
        }
        public async Task<PrivateChat> GetById(Guid Id)
        {
            return await _base.Get(c => c.Id == Id, "Messages,Members");
        }
        //public async Task<List<GroupChat>> GetAllByUserId(){}
        public async Task<SimpleResponseDTO> Create(CreatePrivateChatDTO model)
        {
            // check it is not already created !
            var privateChat = new PrivateChat()
            {
                Members = new List<Users_Private>()
                {
                    new Users_Private(){ MemberId = model.UserId},
                    new Users_Private(){ MemberId = model.UserId2}
                }
            };
            await _base.Create(privateChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Group chat creation is done" };
        }
        /*public async Task<SimpleResponseDTO> Update(Guid PrivateChatId, UpdatePrivateChatDTO model)
        {
            var privateChat = await _base.Get(c => c.Id == PrivateChatId);
            if (privateChat == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Chat is not found" };
            _mapper.Map(privateChat, model);
            await _base.Update(privateChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Group chat creation is done" };
        }*/
        public async Task<SimpleResponseDTO> Delete(Guid GroupChaId)
        {
            var privateChat = await _base.Get(c => c.Id == GroupChaId);
            if (privateChat == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Chat is not found" };
            await _base.Remove(privateChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Chat deletion is done" };
        }
    }
}
