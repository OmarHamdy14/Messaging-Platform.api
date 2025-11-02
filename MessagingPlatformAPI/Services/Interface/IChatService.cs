using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IChatService
    {
        Task<Chat> GetById(Guid Id);
        Task<SimpleResponseDTO> Create(CerateChatDTO model);
        Task<SimpleResponseDTO> Update(Guid ChaId, UpdateChatDTO model);
        Task<SimpleResponseDTO> Delete(Guid ChaId);

        Task<SimpleResponseDTO> LeaveGroupChat(Guid ChatId);
        Task SaveChanges(Chat c);
        Task<SimpleResponseDTO> ChangeChatPic(Chat chat, IFormFile pic);
        Task<SimpleResponseDTO> DeleteChatPic(string ImagePublicId);
    }
}
