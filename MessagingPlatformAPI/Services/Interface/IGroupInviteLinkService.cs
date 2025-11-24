using MessagingPlatformAPI.Helpers.DTOs.GroupInviteLinkDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IGroupInviteLinkService
    {
        Task<SimpleResponseDTO<GroupInviteLink>> CreateLink(CreateLinkDTO model);
        Task<bool> JoinGroup(JoinViaLinkDTO model);
    }
}
