using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IPrivateChatService
    {
        Task<PrivateChat> GetById(Guid Id);
        Task<SimpleResponseDTO> Delete(Guid GroupChaId);
    }
}
