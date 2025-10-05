using MessagingPlatformAPI.Helpers.DTOs.PrivateChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Helpers.UnNeeded
{
    public interface IPrivateChatService
    {
        Task<PrivateChat> GetById(Guid Id);
        Task<SimpleResponseDTO> Create(CreatePrivateChatDTO model);
        Task<SimpleResponseDTO> Delete(Guid GroupChaId);
    }
}
