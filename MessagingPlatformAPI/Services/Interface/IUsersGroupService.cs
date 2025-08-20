using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Helpers.DTOs.UsersGroupDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IUsersGroupService
    {
        Task<List<Users_Group>> GetGroupsOfUser(string UserId);
        Task<List<string>> GetUsersOfGroup(Guid GroupId);
        Task<SimpleResponseDTO> AddUserToGroup(AddUserToGroupDTO model);
        Task<SimpleResponseDTO> RemoveUserFromGroup(RemoveUserFromGroupDTO model);
    }
}
