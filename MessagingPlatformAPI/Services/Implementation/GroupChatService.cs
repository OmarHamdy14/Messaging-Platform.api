using AutoMapper;
using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.GroupChatDTO;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class GroupChatService : IGroupChatService
    {
        private readonly IEntityBaseRepository<GroupChat> _base;
        private readonly IMapper _mapper;
        public GroupChatService(IEntityBaseRepository<GroupChat> @base, IMapper mapper)
        {
            _base = @base;
            _mapper = mapper;
        }
        public async Task<GroupChat> GetById(Guid Id)
        {
            return await _base.Get(c => c.Id == Id, "Messages,Members");
        }
        //public async Task<List<GroupChat>> GetAllByUserId(){}
        public async Task<SimpleResponseDTO> Create(CerateGroupChatDTO model)
        {
            var groupChat = _mapper.Map<GroupChat>(model);
            List<Users_Group> members = new List<Users_Group>();
            foreach(var memberId in model.MmebersId){
                members.Add(new Users_Group() { MemberId = memberId });
            }
            groupChat.Members = members;
            await _base.Create(groupChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message="Group chat creation is done" };
        }
        public async Task<SimpleResponseDTO> Update(Guid GroupChaId, UpdateGroupChatDTO model)
        {
            var groupChat = await _base.Get(c => c.Id == GroupChaId);
            if(groupChat == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Chat is not found" };
            _mapper.Map(groupChat, model); 
            await _base.Update(groupChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Group chat update is done" };
        }
        public async Task<SimpleResponseDTO> Delete(Guid GroupChaId)
        {
            var groupChat = await _base.Get(c => c.Id == GroupChaId);
            if (groupChat == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Chat is not found" };
            await _base.Remove(groupChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Group chat deletion is done" };
        }
    }
}
