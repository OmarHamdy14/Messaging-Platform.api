using MessagingPlatformAPI.Helpers.DTOs.PrivateChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IPrivateChatService
    {
        Task<PrivateChat> GetById(Guid Id);
        Task<SimpleResponseDTO> Create(CreatePrivateChatDTO model);
        Task<SimpleResponseDTO> Delete(Guid GroupChaId);
    }
}
