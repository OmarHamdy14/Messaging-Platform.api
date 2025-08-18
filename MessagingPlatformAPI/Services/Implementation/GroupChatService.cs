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
            return await _base.Get(c => c.Id == Id, "ChatRecords,Members,Admins");
        }
        //public async Task<List<GroupChat>> GetAllByUserId(){}
        public async Task<SimpleResponseDTO> Create(CerateGroupChatDTO model)
        {
            var groupChat = _mapper.Map<GroupChat>(model);
            await _base.Create(groupChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message="Group chat creation is done" };
        }
        public async Task<SimpleResponseDTO> Update(Guid GroupChaId, UpdateGroupChatDTO model)
        {
            var groupChat = await _base.Get(c => c.Id == GroupChaId, "ChatRecords,Members,Admins");
            _mapper.Map(groupChat, model); 
            await _base.Create(groupChat);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Group chat creation is done" };
        }
    }
}
