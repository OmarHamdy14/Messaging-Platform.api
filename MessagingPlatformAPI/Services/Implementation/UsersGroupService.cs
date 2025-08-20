using MessagingPlatformAPI.Base.Interface;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.UsersGroupDTOs;
using MessagingPlatformAPI.Models;
using MessagingPlatformAPI.Services.Interface;

namespace MessagingPlatformAPI.Services.Implementation
{
    public class UsersGroupService : IUsersGroupService
    {
        private readonly IEntityBaseRepository<Users_Group> _base;
        public UsersGroupService(IEntityBaseRepository<Users_Group> @base)
        {
            _base = @base;
        }
        public async Task<List<Users_Group>> GetGroupsOfUser(string UserId)
        {
            return await _base.GetAll(g => g.MemberId == UserId);
        }
        public async Task<List<string>> GetUsersOfGroup(Guid GroupId)
        {
            var res = await _base.GetAll(g => g.GroupChatId == GroupId);
            return res.Select(r => r.MemberId).ToList();
        }
        public async Task<SimpleResponseDTO> AddUserToGroup(AddUserToGroupDTO model)
        {
            var User_Group = new Users_Group()
            {
                MemberId = model.UserId,
                GroupChatId = model.ChatGroupId
            };
            await _base.Create(User_Group);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Adding is done" };
        }
        public async Task<SimpleResponseDTO> RemoveUserFromGroup(RemoveUserFromGroupDTO model)
        {
            var User_Group  = await _base.Get(g => g.MemberId == model.UserId && g.GroupChatId == model.ChatGroupId);
            if(User_Group == null) return new SimpleResponseDTO() { IsSuccess = false, Message = "Recrd is not found" };
            await _base.Remove(User_Group);
            return new SimpleResponseDTO() { IsSuccess = true, Message = "Deletion is done" };
        }
    }
}
