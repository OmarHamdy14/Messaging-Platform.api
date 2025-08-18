using MessagingPlatformAPI.Helpers.DTOs.GroupChatDTO;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IGroupChatService
    {
        Task<GroupChat> GetById(Guid Id);
        Task<SimpleResponseDTO> Create(CerateGroupChatDTO model);
        Task<SimpleResponseDTO> Update(Guid GroupChaId, UpdateGroupChatDTO model);
        Task<SimpleResponseDTO> Delete(Guid GroupChaId);
    }
}
