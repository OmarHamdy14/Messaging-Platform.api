using MessagingPlatformAPI.Helpers.DTOs.ChatDTOs;
using MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs;
using MessagingPlatformAPI.Models;

namespace MessagingPlatformAPI.Services.Interface
{
    public interface IChatService
    {
        Task<Chat> GetById(Guid Id);
        Task<SimpleResponseDTO<Chat>> Create(CerateChatDTO model);
        Task<SimpleResponseDTO<Chat>> Update(Guid ChaId, UpdateChatDTO model);
        Task<SimpleResponseDTO<Chat>> Delete(Guid ChaId);

        Task<SimpleResponseDTO<Chat>> LeaveGroupChat(Guid ChatId);
        Task SaveChanges(Chat c);
        Task<SimpleResponseDTO<ChatImage>> ChangeChatPic(Chat chat, IFormFile pic);
        Task<SimpleResponseDTO<ChatImage>> DeleteChatPic(string ImagePublicId);
    }
}
